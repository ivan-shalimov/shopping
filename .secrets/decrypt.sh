#!/bin/bash

# Script to decrypt all encrypted files back to *.open files
# Uses OpenSSL with AES-256-CBC decryption

SSH_KEY_PATH="$HOME/.ssh/id_ed25519_git_data"
SECRETS_DIR="$(dirname "$0")"

# Check if openssl is available
if ! command -v openssl &> /dev/null; then
    echo "Error: openssl is not available. Please install OpenSSL."
    exit 1
fi

# Check if SSH key exists
if [[ ! -f "$SSH_KEY_PATH" ]]; then
    echo "Error: SSH key not found at $SSH_KEY_PATH"
    exit 1
fi

echo "Using SSH key: $SSH_KEY_PATH"
echo "Secrets directory: $SECRETS_DIR"
echo ""

# Generate encryption key from SSH private key (same as encrypt script)
ENCRYPTION_KEY=$(openssl dgst -sha256 -binary "$SSH_KEY_PATH" | xxd -p -c 256)

if [[ -z "$ENCRYPTION_KEY" ]]; then
    echo "Error: Failed to generate encryption key from SSH key"
    exit 1
fi

# Find all files that don't have .open extension and try to decrypt them
count=0
for encrypted_file in "$SECRETS_DIR"/*; do
    # Skip directories and files that already have .open extension
    [[ ! -f "$encrypted_file" ]] && continue
    [[ "$encrypted_file" == *.open ]] && continue
    [[ "$encrypted_file" == *.sh ]] && continue  # Skip shell scripts
    
    # Get the target filename (add .open extension)
    target_file="${encrypted_file}.open"
    
    echo "Decrypting: $(basename "$encrypted_file") -> $(basename "$target_file")"
    
    # Try to decrypt using OpenSSL AES-256-CBC with PBKDF2
    # First decrypt to a temporary file
    temp_file="${target_file}.tmp"
    
    if openssl enc -d -aes-256-cbc -pbkdf2 -iter 100000 -k "$ENCRYPTION_KEY" -in "$encrypted_file" -out "$temp_file" 2>/dev/null; then
        # Remove the first 16 bytes (salt) from decrypted content
        # The encrypt script prepends 16-byte salt before encryption
        if dd if="$temp_file" of="$target_file" bs=1 skip=16 2>/dev/null; then
            echo "  ✓ Successfully decrypted"
            count=$((count + 1))
        else
            echo "  ✗ Failed to remove salt from decrypted file"
            [[ -f "$target_file" ]] && rm "$target_file"
        fi
        # Clean up temporary file
        rm "$temp_file"
    else
        echo "  ✗ Failed to decrypt (file may not be encrypted or corrupted)"
        # Remove failed output file if it exists
        [[ -f "$target_file" ]] && rm "$target_file"
        [[ -f "$temp_file" ]] && rm "$temp_file"
    fi
done

if [[ $count -eq 0 ]]; then
    echo "No encrypted files found or decrypted in $SECRETS_DIR"
else
    echo ""
    echo "Successfully decrypted $count file(s)"
fi