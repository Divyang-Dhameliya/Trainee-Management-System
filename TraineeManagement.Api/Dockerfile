FROM docker-registry-002.zeuslearning.com/zeuslearning/vscode/devcontainers/dotnet AS api-compiler
 
WORKDIR /src
 
COPY . .

RUN --mount=type=secret,id=aws_token \
    export CODEARTIFACT_TOKEN=$(cat /run/secrets/aws_token) && \
    dotnet restore TraineeManagement.Api.csproj --configfile NuGet.config
  
RUN dotnet publish "TraineeManagement.Api.csproj" \
-c Release \
-o /app/publish 
 
FROM docker-registry-002.zeuslearning.com/zeuslearning/vscode/devcontainers/dotnet
 
WORKDIR /app
 
COPY --from=api-compiler /app/publish .
 
EXPOSE 8080
 
ENTRYPOINT ["dotnet","TraineeManagement.Api.dll"]