#! /bin/bash

echo "Removing CLI breaking .dcproj from solution."
dotnet sln Lazztech.ObsidianPresences.sln remove docker-compose.dcproj
