pipeline {
  agent any
  stages {
    stage('Build') {
      steps {
        echo 'Building Docker'
        sh 'docker build -t atlantis-net-platform .'
      }
    }
    stage('Test') {
      steps {
        echo 'Test'
        sh 'docker run --name net-platform atlantis-net-platform:latest'
      }
    }
    stage('Deploy') {
      steps {
        echo 'Deploying....'
      }
    }
  }
}