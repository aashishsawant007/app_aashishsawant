pipeline {
    agent any
	
	environment {
		scannerHome = tool name: 'sonar_scanner_dotnet'
        dockerhubcredentials = 'dockerhubcredentials'
        username = 'aashishsawant'
        appName = 'NAGP-DevOps'
        registry = 'aashishsawant'
	}
    
    stages {
        stage('Code Checkout'){
            steps {
                echo 'Checkout...' + env.BRANCH_NAME
                git branch: "${env.BRANCH_NAME}", url: 'https://github.com/aashishsawant007/app_aashishsawant.git'
            }
        }
        stage('Nuget Restore'){
            steps {
                bat 'dotnet restore'
            }
        }		      		
        stage('Start SonarQube Analysis'){
               when {
				branch 'master'
			}
            steps {
				echo 'Starting SonarQube Analysis'
				withSonarQubeEnv('Test_Sonar') {
				  bat "dotnet ${scannerHome}\\SonarScanner.MSBuild.dll begin /k:\"sonar-aashishsawant\""
				}
            }
        }
        stage('Code Build'){
            steps {
				bat 'dotnet clean'
                bat 'dotnet build'
            }
        }
        stage('Test Case Execution'){
            steps {
                bat 'dotnet test --logger:trx;LogFileName=appaashishsawanttest.xml'
            }
        }
        stage('Stop SonarQube Analysis'){
               when {
				branch 'master'
			}
            steps {
				echo 'Stopping SonarQube Analysis'
				withSonarQubeEnv('Test_Sonar') {
					bat "dotnet ${scannerHome}\\SonarScanner.MSBuild.dll end" 
				}
            }
        }
        stage('Release Artifact'){
             when {
				branch 'develop'
			}
            steps {
				echo 'Starting Release Artifact'
				bat "dotnet publish -c Release -o ${appname}/app/${username}" 
            }
        }
        stage('Build & Push Docker Image'){
            steps {
				echo 'Starting Build & Push Docker Image'
				script{
					dockerImage = docker.build "${username}/i-${username}-${env.BRANCH_NAME}:latest"
					docker.withRegistry('', env.dockerhubcredentials) {
						dockerImage.push('latest')
					}
				}
            }
        }
        stage('Kubernetes deployment'){
            steps {
				echo 'Starting Kubernetes deployment'
                bat "gcloud auth login"
                bat "kubectl apply -f deployment.yaml"
                bat "kubectl apply -f service.yaml"
            }
        }
    }
}
