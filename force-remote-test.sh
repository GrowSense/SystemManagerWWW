#!/bin/bash

BRANCH=$(git branch | sed -n -e 's/^\* \(.*\)/\1/p')

if [ "$BRANCH" = "dev" ]
then
  echo "Forcing remote test"

  sh clean.sh

  echo " " >> .gitlab-ci.yml
  
  git pull local dev && \
  git commit .gitlab-ci.yml -m "Forcing retest" && \
  git push local dev && \
  
  git pull lan dev && \
  git commit .gitlab-ci.yml -m "Forcing retest" && \
  git push lan dev && \
  
  git pull origin dev && \
  git commit .gitlab-ci.yml -m "Forcing retest" && \
  git push origin dev && \
  
  
  
  echo "Repository has been updated. Test should now start on test server."
else
  echo "Cannot force retest from master branch. Switch to dev branch first."
fi
