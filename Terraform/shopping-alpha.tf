# Server
data "docker_image" "shopping_server_alpha" {
  name = "shopping.server:latest-alpha"
}

resource "docker_container" "shopping_server_alpha" {
  image   = data.docker_image.shopping_server_alpha.id
  name    = "shopping.server-alpha"
  restart = "no"
  must_run = false

  depends_on = [
    docker_network.my-mssql-network,
    docker_network.my-graylog-network
  ]

  env = [
    "ASPNETCORE_ENVIRONMENT=Development",
    "ConnectionStrings__Shopping=Server=mssql;Database=sh-shopping-dev;User=sa;Password=Password1!;Connection Timeout=30;Encrypt=False;Max Pool Size=100;",
    "LoggerSettings__MinimumLevel=Information",
    "LoggerSettings__GraylogSinkOptions__HostnameOrAddress=graylog"
  ]

  ports {
    internal = 8080
    external = 19092
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
data "docker_image" "shopping_client_alpha" {
  name = "shopping.client:latest-alpha"
}



resource "docker_container" "shopping_client_alpha" {
  image   = data.docker_image.shopping_client_alpha.id
  name    = "shopping.client-alpha"
  restart = "no"
  must_run = false

  env = [
    "ASPNETCORE_ENVIRONMENT=Development",
    "API=http://localhost:19092"
  ]

  ports {
    internal = 80
    external = 19091
  }
}