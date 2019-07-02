pipeline {
  agent none
  stages {
    stage('Build Docker') {
      agent {
        dockerfile {
            filename 'Dockerfile'
            dir '.'
        }
      }
      steps {
        echo 'build docker test'
      }
    }
    stage('Test') {
      steps {
        echo 'mvn test'
      }
    }
    stage('Deploy') {
      steps {
        echo 'Deploying....'
      }
    }
  }
}