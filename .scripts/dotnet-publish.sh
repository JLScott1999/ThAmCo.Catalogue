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

docker build -f ./ThAmCo.Catalogue/Dockerfile.$DOCKER_ENV -t thamco.catalogue:$DOCKER_TAG ./ThAmCo.Catalogue --no-cache

docker login docker.io -u $DOCKER_USERNAME -p $DOCKER_PASSWORD
docker tag thamco.catalogue:$DOCKER_TAG docker.io/$DOCKER_USERNAME/thamco.catalogue:$DOCKER_TAG
docker push docker.io/$DOCKER_USERNAME/thamco.catalogue:$DOCKER_TAG

docker login https://docker.pkg.github.com -u $GITHUB_USERNAME -p $GITHUB_PASSWORD
docker tag thamco.catalogue:$DOCKER_TAG docker.pkg.github.com/$GITHUB_USERNAME/thamco.catalogue/thamco.catalogue:$DOCKER_TAG
docker push docker.pkg.github.com/$GITHUB_USERNAME/thamco.catalogue/thamco.catalogue:$DOCKER_TAG