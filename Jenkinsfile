pipeline {
  agent {
    docker {
      image 'microsoft/dotnet:2.1-sdk'
    }

  }
  stages {
    stage('Initialize') {
      steps {
        echo 'Setting up pipeline.'
      }
    }
    stage('Unit Tests') {
      steps {
        sh ''': \'
#!/usr/bin/env bash
cd $(dirname $0)

set -e

docker exec app-dev-dotnet app-test-unit
\''''
      }
    }
  }
}