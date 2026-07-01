FROM docker-registry-002.zeuslearning.com/zeuslearning/vscode/devcontainers/dotnet AS worker-compiler
 
WORKDIR /src
 
COPY SubmisionProcessor.Worker/ SubmisionProcessor.Worker/
COPY TraineeManagement.Api/ TraineeManagement.Api/

WORKDIR /src/SubmisionProcessor.Worker

RUN --mount=type=secret,id=aws_token \
    export CODEARTIFACT_TOKEN=$(cat /run/secrets/aws_token) && \
    dotnet restore SubmisionProcessor.Worker.csproj --configfile NuGet.config
 
RUN dotnet publish "SubmisionProcessor.Worker.csproj" \
-c Release \
-o /app/publish \
/p:ErrorOnDuplicatePublishOutputFiles=false

FROM docker-registry-002.zeuslearning.com/zeuslearning/vscode/devcontainers/dotnet
 
WORKDIR /app
 
COPY --from=worker-compiler /app/publish .
 
ENTRYPOINT ["dotnet","SubmisionProcessor.Worker.dll"]