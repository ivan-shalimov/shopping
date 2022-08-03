docker stop shoppingserver
docker rm shoppingserver
docker run --name=shoppingserver -p 9091:80 -d shoppingserver:latest --restart=always