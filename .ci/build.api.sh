#!/bin/bash
COMMIT_SHA=$(git rev-parse --short HEAD);
echo "start build image 'shopping.server:$COMMIT_SHA'"

# Read GitHub token
GITHUB_TOKEN=$(cat .secrets/github-token.open)

# Login to GitHub Container Registry
echo $GITHUB_TOKEN | docker login ghcr.io -u ivan-shalimov --password-stdin

# Build and tag
docker build -t shopping.server:$COMMIT_SHA -f ./Shopping/Server/Dockerfile . 
docker tag shopping.server:$COMMIT_SHA ghcr.io/ivan-shalimov/shopping.server:$COMMIT_SHA

# Push
echo "push image 'ghcr.io/ivan-shalimov/shopping.server:$COMMIT_SHA'"
docker push ghcr.io/ivan-shalimov/shopping.server:$COMMIT_SHA