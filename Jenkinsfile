pipeline {
    agent any
    options {
      skipDefaultCheckout true
    }
    stages {
        stage('CleanWSStart') {
            steps {
                deleteDir()
            }
        }
        stage('Setup') {
            steps {
                shHide( 'git clone --recursive -b $BRANCH_NAME https://${GHTOKEN}@github.com/GrowSense/SystemManagerWWW.git .' )
                sh 'git config --global user.email "compulsivecoder@gmail.com"'
                sh 'git config --global user.name "CompulsiveCoderCI"'
            }
        }
        stage('Prepare') {
            when { expression { !shouldSkipBuild() } }
            steps {
                sh 'echo "Prepare disabled to speed up tests." # sh prepare.sh'
            }
        }
        stage('Init') {
            when { expression { !shouldSkipBuild() } }
            steps {
                sh 'sh init.sh'
            }
        }
        stage('Build') {
            when { expression { !shouldSkipBuild() } }
            steps {
                sh 'sh build.sh'
            }
        }
        stage('Graduate') {
            when { expression { !shouldSkipBuild() } }
            steps {
                sh 'sh graduate.sh'
            }
        }
        stage('Version') {
            when { expression { !shouldSkipBuild() } }
            steps {
                sh 'sh increment-version.sh'
                sh 'sh push-version.sh'
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
          sh 'sh rollback.sh'
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
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
