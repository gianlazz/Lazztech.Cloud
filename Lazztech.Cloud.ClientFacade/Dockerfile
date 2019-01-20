FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 50472
EXPOSE 44362

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

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["Lazztech.Cloud.ClientFacade/Lazztech.Cloud.ClientFacade.csproj", "Lazztech.Cloud.ClientFacade/"]
RUN dotnet restore "Lazztech.Cloud.ClientFacade/Lazztech.Cloud.ClientFacade.csproj"
COPY . .
WORKDIR "/src/Lazztech.Cloud.ClientFacade"
RUN dotnet build "Lazztech.Cloud.ClientFacade.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Lazztech.Cloud.ClientFacade.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Lazztech.Cloud.ClientFacade.dll"]