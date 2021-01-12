case "$TRAVIS_BRANCH" in
  "main")
    DOCKER_TAG=latest
    ;;
  "develop")
    DOCKER_TAG=dev
    ;;    
esac

DOCKER_TAG=$(grep -A3 'appVersion:' /home/travis/build/JLScott1999/thamco.catalogue/chart/Chart.yaml | sed 's/^.*: //')
echo $DOCKER_TAG

docker build -f ./ThAmCo.Catalogue/Dockerfile -t thamco.catalogue:$DOCKER_TAG ./ThAmCo.Catalogue --no-cache

docker login docker.io -u $DOCKER_USERNAME -p $DOCKER_PASSWORD
docker tag thamco.catalogue:$DOCKER_TAG docker.io/$DOCKER_USERNAME/thamco.catalogue:$DOCKER_TAG
docker push docker.io/$DOCKER_USERNAME/thamco.catalogue:$DOCKER_TAG

docker login https://docker.pkg.github.com -u $GITHUB_USERNAME -p $GITHUB_PASSWORD
docker tag thamco.catalogue:$DOCKER_TAG docker.pkg.github.com/$GITHUB_USERNAME/thamco.catalogue/thamco.catalogue:$DOCKER_TAG
docker push docker.pkg.github.com/$GITHUB_USERNAME/thamco.catalogue/thamco.catalogue:$DOCKER_TAG