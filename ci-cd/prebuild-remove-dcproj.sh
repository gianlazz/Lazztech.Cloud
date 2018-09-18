#! /bin/bash

echo "Changing directory to the root of the git repo."
cd ./$(git rev-parse --show-cdup)

echo "Removing CLI breaking .dcproj from solution."
dotnet sln Lazztech.ObsidianPresences.sln remove docker-compose.dcproj
