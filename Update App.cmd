docker stop shoppingwebapi
docker rm shoppingwebapi
docker run --name=shoppingwebapi -p 9091:80 -d shoppingwebapi:latest --restart=always