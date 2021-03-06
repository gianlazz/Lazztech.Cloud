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
RUN apt-get update -y &&\
    apt-get install -y unzip &&\
    curl -sSL https://aka.ms/getvsdbgsh | bash /dev/stdin -v latest -l ~/vsdbg
WORKDIR /src
COPY Lazztech.Cloud.sln ./
COPY ["Lazztech.Cloud.Vision.Webapi/Lazztech.Cloud.Vision.Webapi.csproj", "Lazztech.Cloud.Vision.Webapi/"]
COPY Lazztech.Cloud.CloudWebApp/Lazztech.Cloud.CloudWebApp.csproj Lazztech.Cloud.CloudWebApp/
COPY Lazztech.Cloud.Vision.Cli/Lazztech.Cloud.Vision.Cli.csproj Lazztech.Cloud.Vision.Cli/
COPY Lazztech.Cloud.Vision.GoogleCloudVision/Lazztech.Cloud.Vision.GoogleCloudVision.csproj Lazztech.Cloud.Vision.GoogleCloudVision/
COPY Lazztech.Cloud.Vision.Domain/Lazztech.Cloud.Vision.Domain.csproj Lazztech.Cloud.Vision.Domain/
COPY Lazztech.Cloud.Vision.Tests/Lazztech.Cloud.Vision.Tests.csproj Lazztech.Cloud.Vision.Tests/
COPY docker-compose.dcproj ./
# RUN dotnet restore -nowarn:msb3202,nu1503
RUN dotnet restore Lazztech.Cloud.sln -nowarn:msb3202,nu1503
COPY . .
WORKDIR /src/Lazztech.Cloud.Vision.Cli
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Lazztech.Cloud.Vision.Cli.dll"]
