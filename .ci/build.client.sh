#!/bin/bash
COMMIT_SHA=$(git rev-parse --short HEAD);
echo "start build image 'shopping.client:$COMMIT_SHA'"

# Read GitHub token
GITHUB_TOKEN=$(cat .secrets/github-token.open)

# Login to GitHub Container Registry
echo $GITHUB_TOKEN | docker login ghcr.io -u ivan-shalimov --password-stdin

# Build and tag
docker build -t shopping.client:$COMMIT_SHA -f ./Shopping/Client/Dockerfile . 
docker tag shopping.client:$COMMIT_SHA ghcr.io/ivan-shalimov/shopping.client:$COMMIT_SHA

# Push
echo "push image 'ghcr.io/ivan-shalimov/shopping.client:$COMMIT_SHA'"
docker push ghcr.io/ivan-shalimov/shopping.client:$COMMIT_SHA