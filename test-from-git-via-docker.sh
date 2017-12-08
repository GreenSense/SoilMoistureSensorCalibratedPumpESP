BRANCH=$1

if [ -z "$BRANCH" ]; then
    BRANCH=$(git branch | sed -n -e 's/^\* \(.*\)/\1/p')
fi

if [ -z "$BRANCH" ]; then
    BRANCH="master"
fi

docker run -it -v $PWD:/Project-src compulsivecoder/ubuntu-platformio /bin/bash -c "git clone /Project-src /Project-dest/ && cd /Project-dest/ && sh init.sh && sh build.sh $BRANCH"
