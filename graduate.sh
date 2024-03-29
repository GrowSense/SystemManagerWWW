#!/bin/bash

BRANCH=$(git branch | sed -n -e 's/^\* \(.*\)/\1/p')

if [ "$BRANCH" = "dev" ];  then
  echo "Graduating dev branch to master branch"

  echo ""
  echo "  Fetching from origin..."
  git fetch origin || exit 1

  echo ""
  echo "  Checking out the master branch..."
  git checkout master || exit 1

  echo ""
  echo "  Merging the dev branch into the master branch..."
  git merge -X theirs origin/dev || exit 1

  echo ""
  echo "  Pushing the updated master branch to origin..."
  bash push.sh -q || exit 1

  echo ""
  echo "  Checking out the dev branch..."
  git checkout dev || exit 1

  echo "The 'dev' branch has been graduated to the 'master' branch"
else
  echo "You must be in the 'dev' branch to graduate to the 'master' branch, but currently in the '$BRANCH' branch. Skipping."
fi
