pipeline {
    agent any
    options {
        disableConcurrentBuilds();
    }
    stages {
        stage('CleanWSStart') {
            steps {
                deleteDir()
            }
        }
        stage('Checkout') {
            steps {
                checkout scm

                shHide( 'git remote set-url origin https://${GHTOKEN}@github.com/GreenSense/SoilMoistureSensorCalibratedPumpESP.git' )
                sh "git config --add remote.origin.fetch +refs/heads/master:refs/remotes/origin/master"
                sh "git fetch --no-tags"
                sh 'git checkout $BRANCH_NAME'
                sh 'git pull origin $BRANCH_NAME'
                shHide( 'sh set-wifi-credentials.sh ${WIFI_NAME} ${WIFI_PASSWORD}' )
                shHide( 'sh set-mqtt-credentials.sh ${MQTT_HOST} ${MQTT_USERNAME} ${MQTT_PASSWORD}' )
            }
        }
        stage('Init') {
            when { expression { !shouldSkipBuild() } }
            steps {
                sh 'sh init.sh'
            }
        }
        stage('Inject Version') {
            when { expression { !shouldSkipBuild() } }
            steps {
                sh 'sh inject-version.sh'
            }
        }
        stage('Build') {
            when { expression { !shouldSkipBuild() } }
            steps {
                sh 'sh build-all.sh'
            }
        }
        stage('CleanWSEnd') {
            steps {
                deleteDir()
            }
        }
    }
    post {
        success() {
          emailext (
              subject: "SUCCESSFUL: Job '${env.JOB_NAME} [${env.BUILD_NUMBER}]'",
              body: """<p>SUCCESSFUL: Job '${env.JOB_NAME} [${env.BUILD_NUMBER}]':</p>
                <p>Check console output at "<a href="${env.BUILD_URL}">${env.JOB_NAME} [${env.BUILD_NUMBER}]</a>"</p>""",
              recipientProviders: [[$class: 'DevelopersRecipientProvider']]
            )
        }
        failure() {
          deleteDir()
          emailext (
              subject: "FAILED: Job '${env.JOB_NAME} [${env.BUILD_NUMBER}]'",
              body: """<p>FAILED: Job '${env.JOB_NAME} [${env.BUILD_NUMBER}]':</p>
                <p>Check console output at "<a href="${env.BUILD_URL}">${env.JOB_NAME} [${env.BUILD_NUMBER}]</a>"</p>""",
              recipientProviders: [[$class: 'DevelopersRecipientProvider']]
            )
        }
    }
}
Boolean shouldSkipBuild() {
    return sh( script: 'sh check-ci-skip.sh', returnStatus: true )
}
def shHide(cmd) {
    sh('#!/bin/sh -e\n' + cmd)
}


 
 
 
 
 
 
