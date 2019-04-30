@Library('jenkins-shared-libraries') _

pipeline {
  agent {
    docker { image 'mcr.microsoft.com/dotnet/core/sdk:2.2' }
  }
  environment {
    dev = 'develop'
    HOME = '/tmp'
  }
  stages {
    stage('Build') {
      steps {
        sh 'dotnet build -c Debug'
        sh 'dotnet build -c Release'
      }
    }
  }
}
