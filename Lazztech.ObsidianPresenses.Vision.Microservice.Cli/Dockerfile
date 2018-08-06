FROM microsoft/dotnet:2.1-runtime AS base

RUN apt-get update -y &&\
    apt-get install -y python3 &&\
    apt-get install -y python3-setuptools &&\
    apt-get install -y python3-dev &&\
    apt-get install -y build-essential cmake &&\
    apt-get install -y libopenblas-dev liblapack-dev &&\
    apt-get install -y git &&\
    git clone https://github.com/davisking/dlib.git &&\
    cd dlib && ls &&\
    python3 setup.py install --yes USE_AVX_INSTRUCTIONS --no DLIB_USE_CUDA &&\
    apt-get install -y python3-pip &&\
    pip3 install face_recognition

ENV LC_ALL C.UTF-8
ENV LANG C.UTF-8
WORKDIR /app

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY Lazztech.ObsidianPresences.sln ./
COPY Lazztech.ObsidianPresenses.Vision.Microservice.Cli/Lazztech.ObsidianPresenses.Vision.Microservice.Cli.csproj Lazztech.ObsidianPresenses.Vision.Microservice.Cli/
COPY Lazztech.ObsidianPresenses.Vision.Microservice.GoogleCloudVision/Lazztech.ObsidianPresenses.Vision.Microservice.GoogleCloudVision.csproj Lazztech.ObsidianPresenses.Vision.Microservice.GoogleCloudVision/
COPY Lazztech.ObsidianPresenses.Vision.Microservice.Domain/Lazztech.ObsidianPresenses.Vision.Microservice.Domain.csproj Lazztech.ObsidianPresenses.Vision.Microservice.Domain/
COPY Lazztech.ObsidianPresenses.Vision.Microservice.Tests/Lazztech.ObsidianPresenses.Vision.Microservice.Tests.csproj Lazztech.ObsidianPresenses.Vision.Microservice.Tests/
COPY docker-compose.dcproj ./
# RUN dotnet restore -nowarn:msb3202,nu1503
RUN dotnet restore Lazztech.ObsidianPresences.sln -nowarn:msb3202,nu1503
COPY . .
WORKDIR /src/Lazztech.ObsidianPresenses.Vision.Microservice.Cli
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Lazztech.ObsidianPresenses.Vision.Microservice.Cli.dll"]
