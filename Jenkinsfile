pipeline {
  agent {
    label 'master'
  }
  stages {
    stage('Build') {
      steps {
        echo 'Building Docker'
        sh 'docker build --no-cache -t atlantis-net-platform .'
      }
    }
    stage('Test') {
      steps {
        echo 'Test'
        sh 'docker login -u jdieuze -p $DOCKER_PASSWORD'
        sh 'docker stop net-platform'
        sh 'docker rm net-platform'
        sh 'docker run --name net-platform atlantis-net-platform:latest test'
        sh 'docker tag atlantis-net-platform:latest jdieuze/net-platform:latest'
        sh 'docker push jdieuze/net-platform:latest'
      }
    }
    stage('Deploy') {
      steps {
        echo 'Deploying....'
      }
    }
  }
}