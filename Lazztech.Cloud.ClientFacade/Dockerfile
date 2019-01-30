FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 50472
EXPOSE 44362

#RUN apt-get update -y &&\
    #apt-get install -y python3 &&\
    #apt-get install -y python3-setuptools &&\
    #apt-get install -y python3-dev &&\
    #apt-get install -y build-essential cmake &&\
    #apt-get install -y libopenblas-dev liblapack-dev &&\
    #apt-get install -y git &&\
    #git clone https://github.com/davisking/dlib.git &&\
    #cd dlib && ls &&\
    #python3 setup.py install --yes USE_AVX_INSTRUCTIONS --no DLIB_USE_CUDA &&\
    #apt-get install -y python3-pip &&\
    #pip3 install face_recognition
#
#ENV LC_ALL C.UTF-8
#ENV LANG C.UTF-8

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY Lazztech.Cloud.sln ./
COPY docker-compose.dcproj ./
COPY ["Lazztech.Cloud.ClientFacade/Lazztech.Cloud.ClientFacade.csproj", "Lazztech.Cloud.ClientFacade/"]
COPY ["Lazztech.Cloud.ClientFacade.Tests/Lazztech.Cloud.ClientFacade.Tests.csproj", "Lazztech.Cloud.ClientFacade.Tests/"]
COPY ["Lazztech.Events/Lazztech.Events.Domain.csproj", "Lazztech.Events/"]
COPY ["Lazztech.Events.Dto/Lazztech.Events.Dto.csproj", "Lazztech.Events.Dto/"]
COPY ["Lazztech.SrndResourcesManager/Lazztech.Events.StudentRND.csproj", "Lazztech.SrndResourcesManager/"]
COPY ["Lazztech.Events.Tests/Lazztech.Events.Tests.csproj", "Lazztech.Events.Tests/"]
COPY ["Lazztech.MongoDB/Lazztech.MongoDB.csproj", "Lazztech.MongoDB/"]
COPY ["Lazztech.Sms/Lazztech.Sms.csproj", "Lazztech.Sms/"]
COPY ["Lazztech.Standard/Lazztech.Standard.csproj", "Lazztech.Standard/"]
COPY ["Lazztech.Vision.Cli/Lazztech.Vision.Cli.csproj", "Lazztech.Vision.Cli/"]
COPY ["Lazztech.Vision.Domain/Lazztech.Vision.Domain.csproj", "Lazztech.Vision.Domain/"]
COPY ["Lazztech.Vision.Tests/Lazztech.Vision.Tests.csproj", "Lazztech.Vision.Tests/"]

RUN dotnet restore Lazztech.Cloud.sln -nowarn:msb3202,nu1503
#RUN dotnet restore "Lazztech.Cloud.ClientFacade/Lazztech.Cloud.ClientFacade.csproj"
COPY . .
WORKDIR "/src/Lazztech.Cloud.ClientFacade"
RUN dotnet build "Lazztech.Cloud.ClientFacade.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Lazztech.Cloud.ClientFacade.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Lazztech.Cloud.ClientFacade.dll"]