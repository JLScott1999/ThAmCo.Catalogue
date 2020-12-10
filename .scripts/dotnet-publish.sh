DOCKER_ENV=''
DOCKER_TAG=''

case "$TRAVIS_BRANCH" in
  "main")
    DOCKER_ENV=Production
    DOCKER_TAG=latest
    ;;
  "develop")
    DOCKER_ENV=Development
    DOCKER_TAG=dev
    ;;    
esac

docker login -u $DOCKER_USERNAME -p $DOCKER_PASSWORD
docker build -f ./ThAmCo.Catalogue/Dockerfile.$DOCKER_ENV -t thamco.catalogue:$DOCKER_TAG ./ThAmCo.Catalogue --no-cache
docker tag thamco.catalogue:$DOCKER_TAG $DOCKER_USERNAME/thamco.catalogue:$DOCKER_TAG
docker push $DOCKER_USERNAME/thamco.catalogue:$DOCKER_TAG