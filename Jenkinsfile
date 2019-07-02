pipeline {
  agent none
  stages {
    stage('Build Docker') {
      agent {
        docker {
            filename 'Dockerfile'
            dir '.'
            label 'net-platform'
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