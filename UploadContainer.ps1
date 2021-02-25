#!/usr/bin/env pwsh

$RepoName="my-repo-name"

$ImageName="word-pluralizer"

docker tag "${ImageName}:latest" "${RepoName}.azurecr.io/${ImageName}:latest"
docker push "${RepoName}.azurecr.io/${ImageName}:latest"
