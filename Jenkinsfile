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
    stage('compose up') {
      steps {
        sh './ci-cd/docker-compose-up.sh'
      }
    }
    stage('compose down') {
      steps {
        sh './ci-cd/docker-compose-down.sh'
      }
    }
    // stage('Deploy to cluster') {
    //     when {
    //         branch 'deploy-to-cluster'
    //     }
    //     steps {
    //         sh './ci-cd/deploy-to-cluster.sh'
    //         input message: 'Finished using the web site? (Click "Proceed" to continue)'
    //     }
    }
  }
}