FROM microsoft/dotnet:2.1-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY Lazztech.ObsidianPresences.sln ./
COPY Lazztech.ObsidianPresenses.Vision.Microservice.Webapi/Lazztech.ObsidianPresenses.Vision.Microservice.Webapi.csproj Lazztech.ObsidianPresenses.Vision.Microservice.Webapi/
RUN dotnet restore -nowarn:msb3202,nu1503
COPY . .
WORKDIR /src/Lazztech.ObsidianPresenses.Vision.Microservice.Webapi
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Lazztech.ObsidianPresenses.Vision.Microservice.Webapi.dll"]
