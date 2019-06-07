#!/bin/bash

BRANCH=$(git branch | sed -n -e 's/^\* \(.*\)/\1/p')

if [ "$BRANCH" = "lts" ]
then
  echo "Rolling back the master branch to previous revision"

  echo "Reverting to previous revision" && \
  git reset --hard HEAD~1 && \

  echo "Pushing back to origin/master" && \
  git push --force origin master && \

  echo "The 'master' branch has been rolled back"  || exit 1
else
  echo "You must be in the 'lts' branch to perform a rollback. Rollback skipped."
fi

