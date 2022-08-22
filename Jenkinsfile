pipeline {
    agent any
	
	environment {
		scannerHome = tool name: 'sonar_scanner_dotnet'
        dockerhubcredentials = 'dockerhubcredentials'
	}
    
    stages {
        stage('Code Checkout'){
            steps {
                echo 'Pulling...' + env.BRANCH_NAME
                git branch: 'master', url: 'https://github.com/aashishsawant007/app_aashishsawant.git'
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
					dockerImage = docker.build 'aashishsawant/i-aashishsawant-master:latest'
					docker.withRegistry('', dockerhubcredentials) {
						dockerImage.push('latest')
					}
				}
            }
        }
    }
}
