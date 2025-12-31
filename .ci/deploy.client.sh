#!/bin/bash

# Exit on any error
set -e

# Configuration
export COMMIT_SHA=$(git rev-parse --short HEAD)
export API_URL=${API_URL:-"http://192.168.0.100:9092"}
export NAMESPACE=${NAMESPACE:-"main"}
export GITHUB_USERNAME=${GITHUB_USERNAME:-"ivan.shalimov"}
export GITHUB_EMAIL=${GITHUB_EMAIL:-"ivan.shalimov@outlook.com"}

ARTIFACTS_DIR="./artifacts"
SECRETS_FILE=".secrets/github-token.open"

echo "Starting deploy for image 'shopping.client:$COMMIT_SHA'"
echo "API URL: $API_URL"
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

# Check if artifacts/client directory exists
if [[ ! -d "$ARTIFACTS_DIR/client" ]]; then
    mkdir -p "$ARTIFACTS_DIR/client"
    echo "Created artifacts directory: $ARTIFACTS_DIR/client"
fi

# construct kube configuration file
echo "Processing Kubernetes templates..."

for open_file in .ci/kube_templates/client/*; do
    [[ ! -f "$open_file" ]] && continue
    
    FILE_NAME=$(basename "$open_file")
    echo "  Processing: $FILE_NAME"
    envsubst < ".ci/kube_templates/client/$FILE_NAME" > "$ARTIFACTS_DIR/client/$FILE_NAME"
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

# Clear token from memory
unset GITHUB_TOKEN

# Apply Kubernetes manifests
echo "Applying Kubernetes manifests..."
kubectl apply -f "$ARTIFACTS_DIR/client/" -n "$NAMESPACE"

echo "Deployment completed successfully!"