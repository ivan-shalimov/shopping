docker compose --env-file ./config/.env.prod down
docker compose --env-file ./config/.env.prod build shopping.server
docker compose --env-file ./config/.env.prod build shopping.client
docker compose --env-file ./config/.env.prod up -d