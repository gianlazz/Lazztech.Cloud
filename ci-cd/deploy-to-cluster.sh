#! /bin/bash

echo "Deploying to lazztech arm cluster."

echo "Building docker-compose images."
docker-compose -f .\docker-compose.rpi-cluster-prod.yml build
echo "Pushing docker-compose images to the public docker hub container registry."
docker-compose -f .\docker-compose.rpi-cluster-prod.yml push

echo "Ssh'ing into master pi node."
ssh pi@192.168.0.100
echo "Pulling docker -compose images."
docker-compose -f docker-compose.rpi-cluster-prod.yml pull
echo "Launching docker-compose stack accross swarm cluster with stack deploy."
docker stack deploy -c docker-compose.rpi-cluster-prod.yml lazztech-cloud

echo "Pinging cloud.lazz.tech/ to ensure it's up."
ping http://cloud.lazz.tech/
