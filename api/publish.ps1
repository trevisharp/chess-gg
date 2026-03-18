# aws ecr create-repository --repository-name chess-api
# aws ecr get-login-password --region sa-east-1 | docker login --username AWS --password-stdin 771358505503.dkr.ecr.sa-east-1.amazonaws.com/chess-api

docker build -t chess-api .
docker tag chess-api:latest 771358505503.dkr.ecr.sa-east-1.amazonaws.com/chess-api
docker push 771358505503.dkr.ecr.sa-east-1.amazonaws.com/chess-api:latest