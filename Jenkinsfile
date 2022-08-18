pipeline {
    agent any
	
    environment {
		username = 'aashishsawant'
		appname = 'DevOpsAssignment'
	}
    
    stages {
        stage('Code Checkout'){
            steps {

                git branch: 'develop', url: 'https://github.com/aashishsawant007/app_aashishsawant.git'
            }
        }
        stage('Nuget Restore'){
            steps {
                bat 'dotnet restore'
            }
        }		      		
        stage('Code Build'){
            steps {
				bat 'dotnet clean'
                bat 'dotnet build'
            }
        }
        stage('Release Artifact'){
            steps {
				echo 'Starting Release Artifact'
				bat "dotnet publish -c Release -o ${appname}/app/${username}" 
            }
        }
    }
}
