pipeline {
  agent {
    docker {
      image 'microsoft/dotnet:2.1-sdk'
    }

  }
  stages {
    stage('cd to Git Root') {
      steps {
        sh './ci-cd/cd-to-repo-root.sh'
      }
    }
    stage('Remove .dcproj') {
      steps {
        sh './ci-cd/prebuild-remove-dcproj.sh'
      }
    }
    stage('Restore and Build') {
      steps {
        sh './ci-cd/restore-and-build.sh'
      }
    }
    stage('Unit Tests') {
      steps {
        sh './ci-cd/run-unit-tests.sh'
      }
    }
  }
}