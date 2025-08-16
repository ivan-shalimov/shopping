# Server
data "docker_image" "shopping_server" {
  name = "shopping.server:latest"
}

resource "docker_container" "shopping_server" {
  image   = data.docker_image.shopping_server.id
  name    = "shopping.server"
  restart = "always"

  depends_on = [
    docker_network.my-mssql-network,
    docker_network.my-graylog-network
  ]

  env = [
    "ASPNETCORE_ENVIRONMENT=Production",
    "ConnectionStrings__Shopping=Server=mssql;Database=sh-shopping;User=sa;Password=Password1!;Connection Timeout=30;Encrypt=False;Max Pool Size=100;",
    "LoggerSettings__MinimumLevel=Information",
    "LoggerSettings__GraylogSinkOptions__HostnameOrAddress=graylog"
  ]

  ports {
    internal = 8080
    external = 9092
  }

  networks_advanced {
    name = "my-mssql-network"
  }
  networks_advanced {
    name = "my-graylog-network"
  }
  networks_advanced {
    name = "my-metrics-network"
  }
}

#Client
data "docker_image" "shopping_client" {
  name = "shopping.client:latest"
}

resource "docker_container" "shopping_client" {
  image   = data.docker_image.shopping_client.id
  name    = "shopping.client"
  restart = "always"

  env = [
    "ASPNETCORE_ENVIRONMENT=Production",
    "API=http://localhost:9092"
  ]

  ports {
    internal = 80
    external = 9091
  }
}