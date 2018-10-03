FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 50199
EXPOSE 5000

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

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["Lazztech.ObsidianPresences.Vision.Microservice.Webapi/Lazztech.ObsidianPresences.Vision.Microservice.Webapi.csproj", "Lazztech.ObsidianPresences.Vision.Microservice.Webapi/"]
COPY Lazztech.ObsidianPresences.CloudWebApp/Lazztech.ObsidianPresences.CloudWebApp.csproj Lazztech.ObsidianPresences.CloudWebApp/
COPY Lazztech.ObsidianPresences.Vision.Microservice.Cli/Lazztech.ObsidianPresences.Vision.Microservice.Cli.csproj Lazztech.ObsidianPresences.Vision.Microservice.Cli/
COPY Lazztech.ObsidianPresences.Vision.Microservice.GoogleCloudVision/Lazztech.ObsidianPresences.Vision.Microservice.GoogleCloudVision.csproj Lazztech.ObsidianPresences.Vision.Microservice.GoogleCloudVision/
COPY Lazztech.ObsidianPresences.Vision.Microservice.Domain/Lazztech.ObsidianPresences.Vision.Microservice.Domain.csproj Lazztech.ObsidianPresences.Vision.Microservice.Domain/
COPY Lazztech.ObsidianPresences.Vision.Microservice.Tests/Lazztech.ObsidianPresences.Vision.Microservice.Tests.csproj Lazztech.ObsidianPresences.Vision.Microservice.Tests/
COPY docker-compose.dcproj .
RUN dotnet restore "Lazztech.ObsidianPresences.Vision.Microservice.Webapi/Lazztech.ObsidianPresences.Vision.Microservice.Webapi.csproj"
COPY . .
WORKDIR "/src/Lazztech.ObsidianPresences.Vision.Microservice.Webapi"
RUN dotnet build "Lazztech.ObsidianPresences.Vision.Microservice.Webapi.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Lazztech.ObsidianPresences.Vision.Microservice.Webapi.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Lazztech.ObsidianPresences.Vision.Microservice.Webapi.dll"]