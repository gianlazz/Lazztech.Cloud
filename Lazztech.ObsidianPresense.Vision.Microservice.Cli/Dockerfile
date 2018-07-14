FROM microsoft/dotnet:2.1-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.1-sdk AS build
#RUN apt-get update -y && apt-get install -y software-properties-common && add-apt-repository ppa:deadsnakes/ppa && apt-get install python3 -y
#RUN apt-get install -y software-properties-common
#RUN add-apt-repository ppa:deadsnakes/ppa
RUN apt-get update -y && apt-get install python3 -y
WORKDIR /src
COPY Lazztech.ObsidianPresences.sln ./
COPY Lazztech.ObsidianPresense.Vision.Microservice.Cli/Lazztech.ObsidianPresense.Vision.Microservice.Cli.csproj Lazztech.ObsidianPresense.Vision.Microservice.Cli/
COPY Lazztech.ObsidianPresense.Vision.Microservice.GoogleCloudVision/Lazztech.ObsidianPresense.Vision.Microservice.GoogleCloudVision.csproj Lazztech.ObsidianPresense.Vision.Microservice.GoogleCloudVision/
COPY Lazztech.ObsidianPresense.Vision.Microservice.Domain/Lazztech.ObsidianPresense.Vision.Microservice.Domain.csproj Lazztech.ObsidianPresense.Vision.Microservice.Domain/
RUN dotnet restore -nowarn:msb3202,nu1503
COPY . .
WORKDIR /src/Lazztech.ObsidianPresense.Vision.Microservice.Cli
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Lazztech.ObsidianPresense.Vision.Microservice.Cli.dll"]
