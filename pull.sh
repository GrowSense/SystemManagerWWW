BRANCH=$(git branch | sed -n -e 's/^\* \(.*\)/\1/p')

git pull lan $BRANCH || echo "Failed to pull from lan repository"

git pull origin $BRANCH
