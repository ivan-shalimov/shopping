docker compose --env-file ./config/.env.dev down
docker compose --env-file ./config/.env.dev build shopping.client
docker compose --env-file ./config/.env.dev up -d