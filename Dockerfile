FROM mcr.microsoft.com/dotnet/sdk:6.0 AS installer-env

COPY . /src/dotnet-function-app


RUN cd /src/dotnet-function-app && \
    mkdir -p /home/site/wwwroot && \
    dotnet publish *.csproj --output /home/site/wwwroot

# To enable ssh & remote debugging on app service change the base image to the one below
# FROM mcr.microsoft.com/azure-functions/dotnet:4-appservice
FROM mcr.microsoft.com/azure-functions/dotnet:4-appservice
ENV AzureWebJobsScriptRoot=/home/site/wwwroot \
    AzureFunctionsJobHost__Logging__Console__IsEnabled=true
ENV TELEGRAF_CONFIG_PATH=$CONF_URL
COPY --from=installer-env ["/home/site/wwwroot", "/home/site/wwwroot"]