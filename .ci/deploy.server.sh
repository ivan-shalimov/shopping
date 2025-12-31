#!/bin/bash

# Exit on any error
set -e

# Configuration
set -a       # Automatically exports all subsequent variables
source .secrets/shopping.server.open
set +a

export COMMIT_SHA=$(git rev-parse --short HEAD)
export NAMESPACE=${NAMESPACE:-"main"}
export GITHUB_USERNAME=${GITHUB_USERNAME:-"ivan.shalimov"}
export GITHUB_EMAIL=${GITHUB_EMAIL:-"ivan.shalimov@outlook.com"}

# Base64 encode database credentials for Kubernetes secret
export DB_CONNECTION_B64=$(echo -n "$DB_CONNECTION" | base64 -w 0)

ARTIFACTS_DIR="./artifacts"
SECRETS_FILE=".secrets/github-token.open"

echo "Starting deploy for image 'shopping.server:$COMMIT_SHA'"
echo "Namespace: $NAMESPACE"
echo ""

# Validation
if [[ ! -f "$SECRETS_FILE" ]]; then
    echo "Error: GitHub token file not found at $SECRETS_FILE"
    echo "Run: .secrets/decrypt.sh to decrypt your secrets first"
    exit 1
fi

if ! command -v kubectl &> /dev/null; then
    echo "Error: kubectl is not installed or not in PATH"
    exit 1
fi

if ! command -v envsubst &> /dev/null; then
    echo "Error: envsubst is not installed"
    exit 1
fi

# Check if artifacts directory exists
if [[ ! -d "$ARTIFACTS_DIR" ]]; then
    mkdir -p "$ARTIFACTS_DIR"
    echo "Created artifacts directory: $ARTIFACTS_DIR"
fi

# Check if artifacts/server directory exists
if [[ ! -d "$ARTIFACTS_DIR/server" ]]; then
    mkdir -p "$ARTIFACTS_DIR/server"
    echo "Created artifacts directory: $ARTIFACTS_DIR/server"
fi

# construct kube configuration file
echo "Processing Kubernetes templates..."

for open_file in .ci/kube_templates/server/*; do
    [[ ! -f "$open_file" ]] && continue
    
    FILE_NAME=$(basename "$open_file")
    echo "  Processing: $FILE_NAME"
    envsubst < ".ci/kube_templates/server/$FILE_NAME" > "$ARTIFACTS_DIR/server/$FILE_NAME"
done

# Read GitHub token securely
echo "Setting up GitHub Container Registry secret..."
GITHUB_TOKEN=$(cat "$SECRETS_FILE")

if [[ -z "$GITHUB_TOKEN" ]]; then
    echo "Error: GitHub token is empty"
    exit 1
fi

# Update Kubernetes secret for GitHub Container Registry
echo "Removing existing secret..."
kubectl delete secret ghcr-secret -n "$NAMESPACE" --ignore-not-found=true

echo "Creating new GitHub Container Registry secret..."
kubectl create secret docker-registry ghcr-secret \
    --docker-server=ghcr.io \
    --docker-username="$GITHUB_USERNAME" \
    --docker-password="$GITHUB_TOKEN" \
    --docker-email="$GITHUB_EMAIL" \
    -n "$NAMESPACE"

# Clear sensitive data from memory
unset GITHUB_TOKEN
unset DB_USERNAME
unset DB_PASSWORD
unset DB_USERNAME_B64
unset DB_PASSWORD_B64

# Apply Kubernetes manifests
echo "Applying Kubernetes manifests..."
kubectl apply -f "$ARTIFACTS_DIR/server/" -n "$NAMESPACE"
kubectl rollout restart deployment api-shopping-dpl -n "$NAMESPACE"

echo "Server deployment completed successfully!"