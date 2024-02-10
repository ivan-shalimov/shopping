# Volumes
# The follwoing volumes should exist or will be created automaticaly
# my_mssql_volume

resource "docker_image" "mssql" {
  name         = "mcr.microsoft.com/mssql/server:2019-latest"
  #keep_locally = false
}

resource "docker_network" "my-mssql-network" {
  name = "my-mssql-network"
}

resource "docker_container" "mssql" {
  image = docker_image.mssql.image_id
  name  = "my-mssql"
  restart = "always"

  depends_on = [ 
    docker_network.my-mssql-network
   ]

  env = [ 
    "SA_PASSWORD=Password1!",
    "ACCEPT_EULA=Y",
    "MSSQL_PID=Express"
   ]

  ports {
    internal = 1433
    external = 11433
  }

  networks_advanced {
    name = "my-mssql-network"
    aliases = [ "mssql" ]
  }

  volumes {
    volume_name = "my_mssql_volume"
    container_path = "/var/opt/mssql"
  }
}