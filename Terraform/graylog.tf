# Volumes
# The follwoing volumes must exist
# my_graylog_mongo_db
# my_graylog_mongo_configdb
# my_graylog_es_data
# my_graylog_data

# Network
resource "docker_network" "my-graylog-network" {
  name = "my-graylog-network"
}

# MongoDB: https://hub.docker.com/_/mongo/
resource "docker_image" "mongo" {
  name         = "mongo:5.0.13"
  #keep_locally = false
}

resource "docker_container" "mongo" {
  image   = docker_image.mongo.image_id
  name    = "my-mongo"
  restart = "always"
  network_mode = "bridge"

  depends_on = [
    docker_network.my-graylog-network
  ]

  networks_advanced {
    name = "my-graylog-network"
    aliases = [ "mongo" ]
  }

  volumes {
    volume_name    = "my_graylog_mongo_db"
    container_path = "/data/db"
  }
  volumes {
    volume_name    = "my_graylog_mongo_configdb"
    container_path = "/data/configdb"
  }

}

# Elasticsearch: https://www.elastic.co/guide/en/elasticsearch/reference/7.10/docker.html
resource "docker_image" "elasticsearch" {
  name         = "docker.elastic.co/elasticsearch/elasticsearch-oss:7.10.2"
  #keep_locally = false
}

resource "docker_container" "elasticsearch" {
  image   = docker_image.elasticsearch.image_id
  name    = "my-elasticsearch"
  restart = "always"
  network_mode = "bridge"

  depends_on = [
    docker_network.my-graylog-network
  ]

  env = [
    "http.host=0.0.0.0",
    "transport.host=localhost",
    "network.host=0.0.0.0",
    "ES_JAVA_OPTS=-Dlog4j2.formatMsgNoLookups=true -Xms512m -Xmx512m"
  ]

  ulimit {
    name = "memlock"
    hard = -1
    soft = -1
  }
  memory = 1024
  memory_swap = 2048

  networks_advanced {
    name    = "my-graylog-network"
    aliases = ["elasticsearch"]
  }

  volumes {
    volume_name    = "my_graylog_es_data"
    container_path = "/usr/share/elasticsearch/data"
  }
}

# Graylog: https://hub.docker.com/r/graylog/graylog/
resource "docker_image" "graylog" {
  name         = "graylog/graylog:5.0"
  #keep_locally = false
}

resource "docker_container" "graylog" {
  image   = docker_image.graylog.image_id
  name    = "my-graylog"
  restart = "always"
  network_mode = "bridge"

  depends_on = [
    docker_network.my-graylog-network,
    docker_container.elasticsearch
  ]

  env = [
    # CHANGE ME (must be at least 16 characters)!
    "GRAYLOG_PASSWORD_SECRET=somepasswordpepper",
    # Password: admin
    "GRAYLOG_ROOT_PASSWORD_SHA2=8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918",
    "GRAYLOG_HTTP_EXTERNAL_URI=http://127.0.0.1:9000/",
    "GRAYLOG_ELASTICSEARCH_HOSTS=http://elasticsearch:9200"
  ]

  # Graylog web interface and REST API
  ports {
    internal = 9000
    external = 9000
  }
  # GELF TCP
  ports {
    internal = 12201
    external = 12201
  }
  # GELF UDP
  ports {
    internal = 12201
    external = 12201
    protocol = "udp"
  }

  networks_advanced {
    name    = "my-graylog-network"
    aliases = ["graylog"]
  }

  volumes {
    volume_name    = "my_graylog_data"
    container_path = "/usr/share/graylog/data"
  }
}
