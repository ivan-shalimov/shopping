# Volumes
# The follwoing volumes should exist or will be created automaticaly
# my_grafana_data


resource "docker_network" "my-metrics-network" {
  name = "my-metrics-network"
}

# grafana: https://grafana.com/docs/grafana/latest/setup-grafana/installation/docker/
resource "docker_image" "grafana" {
  name = "grafana/grafana-oss:8.5.22"
}

resource "docker_container" "grafana" {
  image = docker_image.grafana.image_id
  name  = "my-grafana"
  restart = "always"
  network_mode = "bridge"

  depends_on = [ 
    docker_network.my-metrics-network
   ]

  env = [ 
    "GF_SECURITY_ADMIN_PASSWORD=secret",
    "GF_USERS_ALLOW_SIGN_UP=false"
   ]

  ports {
    internal = 3000
    external = 9003
  }

  networks_advanced {
    name = "my-metrics-network"
  }

  volumes {
    volume_name = "my_grafana_data"
    container_path = "/var/lib/grafana"
  }
}

# prometheus
resource "docker_image" "prometheus" {
  name = "prom/prometheus"
}

resource "docker_container" "prometheus" {
  image = docker_image.prometheus.image_id
  name  = "my-prometheus"
  restart = "always"
  network_mode = "bridge"

  depends_on = [ 
    docker_network.my-metrics-network
   ]

  env = [ 
    "GF_SECURITY_ADMIN_PASSWORD=secret",
    "GF_USERS_ALLOW_SIGN_UP=false"
   ]

  ports {
    internal = 9090
    external = 9004
  }

  networks_advanced {
    name = "my-metrics-network"
  }

  volumes {
    volume_name = "my_prometheus_data"
    container_path = "/prometheus"
  }
# to configure endpoints go to /etc/prometheus/prometheus.yml and set the following
# # my global config
# global:
#   scrape_interval:     60s # Set the scrape interval to every 15 seconds. Default is every 1 minute.
#   evaluation_interval: 180s # Evaluate rules every 15 seconds. The default is every 1 minute.
#   # scrape_timeout is set to the global default (10s).

#   # Attach these labels to any time series or alerts when communicating with
#   # external systems (federation, remote storage, Alertmanager).
#   # external_labels:
#   #     monitor: 'codelab-monitor'

# # Load rules once and periodically evaluate them according to the global 'evaluation_interval'.
# rule_files:
#   # - "first.rules"
#   # - "second.rules"

# # A scrape configuration containing exactly one endpoint to scrape:
# scrape_configs:
#   # The job name is added as a label `job=<job_name>` to any timeseries scraped from this config.
#   - job_name: 'shopping-dev'

#     # metrics_path defaults to '/metrics'
#     # scheme defaults to 'http'.

#     static_configs:
#       - targets: ['shopping.server-alpha:80']

}