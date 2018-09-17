pipeline {
  agent {
    docker {
      image 'microsoft/dotnet:2.1-sdk'
    }

  }
  stages {
    stage('Remove CLI breaking .dcproj') {
      steps {
        sh './ci-cd/prebuild-remove-dcproj.sh'
      }
    }
    stage('Restore and build') {
      steps {
        sh './ci-cd/restore-and-build.sh'
      }
    }
    stage('Unit Tests') {
      steps {
        echo 'Placeholder for unit tests'
      }
    }
  }
}