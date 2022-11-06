docker compose down
docker compose build shopping.server
docker compose -f docker-compose.yml -f docker-compose.prod.yml up -d