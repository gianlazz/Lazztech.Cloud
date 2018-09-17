#! /bin/bash

dotnet restore
dotnet build
#dotnet publish -c release
#docker build -t dockerhubuser/simplecoreapp:v0.${BUILD_NUMBER} .
#docker login -u dockerhubuser -p dockerhubpassword -e user@domain.com
#docker push dockerhubuser/simplecoreapp:v0.${BUILD_NUMBER}dotnet restore
#dotnet publish -c release
#docker build -t dockerhubuser/simplecoreapp:v0.${BUILD_NUMBER} .
#docker login -u dockerhubuser -p dockerhubpassword -e user@domain.com
#docker push dockerhubuser/simplecoreapp:v0.${BUILD_NUMBER}
