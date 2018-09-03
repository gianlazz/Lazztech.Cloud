FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 50199
EXPOSE 5000

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["Lazztech.ObsidianPresences.Vision.Microservice.Webapi/Lazztech.ObsidianPresences.Vision.Microservice.Webapi.csproj", "Lazztech.ObsidianPresences.Vision.Microservice.Webapi/"]
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