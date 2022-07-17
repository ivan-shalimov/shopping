docker stop shoppingwebapi-latest
docker rm shoppingwebapi-latest
docker run --name=shoppingwebapi-latest -p 9091:80 -d shoppingwebapi:latest --restart=always