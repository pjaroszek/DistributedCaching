FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY DistributedCachingPoC.sln DistributedCachingPoC.sln
COPY DistributedCachingPoC.Core/DistributedCachingPoC.Core.csproj DistributedCachingPoC.Core/DistributedCachingPoC.Core.csproj
COPY DistributedCachingPoC.Application/DistributedCachingPoC.Application.csproj DistributedCachingPoC.Application/DistributedCachingPoC.Application.csproj
COPY DistributedCachingPoC.Infrastructure/DistributedCachingPoC.Infrastructure.csproj DistributedCachingPoC.Infrastructure/DistributedCachingPoC.Infrastructure.csproj
COPY DistributedCachingPoC.WebUI/DistributedCachingPoC.WebUI.csproj DistributedCachingPoC.WebUI/DistributedCachingPoC.WebUI.csproj
RUN dotnet restore

COPY . .

RUN dotnet build -c Release -o /app/build
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app

RUN apt-get update && apt-get install -y net-tools iputils-ping telnet

COPY --from=build /app/publish .

RUN groupadd -g 10000 dotnet && useradd -u 10000 -g dotnet dotnet && chown -R dotnet:dotnet /app
USER dotnet:dotnet

ENV ASPNETCORE_URLS http://*:5009
EXPOSE 5009

ENTRYPOINT ["dotnet", "DistributedCachingPoC.WebUI.dll"]