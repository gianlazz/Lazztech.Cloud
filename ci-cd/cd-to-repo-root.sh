#! /bin/bash

echo "Changing directory to the root of the git repo."
cd ./$(git rev-parse --show-cdup)
