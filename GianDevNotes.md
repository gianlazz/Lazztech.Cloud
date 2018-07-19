##### Docker notes
docker pull gianlazzarini/lazztech_containerizedface_recognition

docker ps

docker run -it -v "Full path to images directory without quotes" --entrypoint /bin/bash "Image id without quotes"

docker run -it -v /Users/gianlazzarini/Desktop/face_recognition:/face/ --entrypoint /bin/bash ba34ace8a4cc

docker system prune -a

Docker links:
https://stackoverflow.com/questions/39988844/docker-compose-up-vs-docker-compose-up-build-vs-docker-compose-build-no-cach
https://www.digitalocean.com/community/tutorials/how-to-remove-docker-images-containers-and-volumes