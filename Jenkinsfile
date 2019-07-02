pipeline {
  agent {
        dockerfile {
            filename 'Dockerfile'
            dir '.'
            container-name 'net-platform'
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