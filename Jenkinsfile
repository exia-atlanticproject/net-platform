pipeline {
  agent {
        dockerfile {
            filename 'Dockerfile'
            dir '.'
            name 'net-platform'
        }
      }
  stages {
    stage('Build') {
      steps {
        echo 'build'
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