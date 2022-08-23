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
        stage('Docker Image') {
			steps {
				echo "Create Docker Image"
				bat "docker build -t i-${username}-${BRANCH_NAME}:${BUILD_NUMBER} --no-cache -f Dockerfile ."
                echo 'Tagging Docker Image'
                bat "docker tag i-${username}-${BRANCH_NAME} ${registry}:${BRANCH_NAME}-${BUILD_NUMBER}"
                bat "docker tag i-${username}-${BRANCH_NAME} ${registry}:${BRANCH_NAME}-latest"

                echo 'Pushing Image to Docker Hub'
                withDockerRegistry([credentialsId: env.dockerhubcredentials, url: '']) {
                    bat "docker push ${registry}:${BRANCH_NAME}-${BUILD_NUMBER}"
                    bat "docker push ${registry}:${BRANCH_NAME}-latest"
                }
		}
        stage('Kubernetes deployment'){
            steps {
				echo 'Starting Kubernetes deployment'
            }
        }
    }
}
