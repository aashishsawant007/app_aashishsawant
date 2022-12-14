pipeline {
    agent any
	
	environment {
		scannerHome = tool name: 'sonar_scanner_dotnet'
        credentialId = 'dockerhubcredentials'
        username = 'aashishsawant'
        appName = 'NAGP-DevOps'
        registry = 'aashishsawant'
        clusterName='kubernetes-cluster-aashishsawant'
        gcloudProject='lucky-adviser-357404'
        zone='us-central1-c'
	}
    
    stages {
        stage('Nuget Restore'){
            steps {
                echo 'Checkout...' + env.BRANCH_NAME
                git branch: "${env.BRANCH_NAME}", url: 'https://github.com/aashishsawant007/app_aashishsawant.git'
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
        stage('Kubernetes deployment'){
            steps {
                echo 'Starting Kubernetes deployment'
				script{
					dockerImage = docker.build "${username}/i-${username}-${env.BRANCH_NAME}:latest"
					docker.withRegistry('', env.credentialId) {
						dockerImage.push('latest')
					}
				}
                bat "gcloud auth login"
                bat "gcloud container clusters get-credentials ${clusterName} --zone ${zone} --project ${gcloudProject}"
                bat "kubectl apply -f deployment.yaml"
                bat "kubectl apply -f service.yaml"
                echo 'Kubernetes deployment done'
            }
        }
    }
}
