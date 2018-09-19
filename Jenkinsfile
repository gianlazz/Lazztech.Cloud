pipeline {
  agent {
    docker {
      image 'gianlazzarini/lazztech_cicd_build'
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
    stage('docker-compose up') {
      steps {
        sh './ci-cd/docker-compose-up.sh'
      }
    }
  }
}