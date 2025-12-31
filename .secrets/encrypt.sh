#!/bin/bash

# Script to encrypt all *.open files using SSH key
# Uses OpenSSL with AES-256-CBC encryption

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

# Generate encryption key from SSH private key
# Use SHA-256 hash of the SSH key as the encryption key
ENCRYPTION_KEY=$(openssl dgst -sha256 -binary "$SSH_KEY_PATH" | xxd -p -c 256)

if [[ -z "$ENCRYPTION_KEY" ]]; then
    echo "Error: Failed to generate encryption key from SSH key"
    exit 1
fi

# Find and encrypt all *.open files
count=0
for open_file in "$SECRETS_DIR"/*.open; do
    # Skip if no .open files found
    [[ ! -f "$open_file" ]] && continue
    
    # Get the target filename (remove .open extension)
    target_file="${open_file%.open}"
    
    echo "Encrypting: $(basename "$open_file") -> $(basename "$target_file")"
    
    # Generate a random salt for each file
    SALT=$(openssl rand -hex 16)
    
    # Encrypt using OpenSSL AES-256-CBC with PBKDF2
    if echo -n "$SALT" | xxd -r -p | cat - "$open_file" | \
       openssl enc -aes-256-cbc -pbkdf2 -iter 100000 -k "$ENCRYPTION_KEY" -out "$target_file"; then
        echo "  ✓ Successfully encrypted with salt: $SALT"
        count=$((count + 1))
    else
        echo "  ✗ Failed to encrypt"
    fi
done

if [[ $count -eq 0 ]]; then
    echo "No *.open files found in $SECRETS_DIR"
else
    echo ""
    echo "Successfully encrypted $count file(s)"
fi