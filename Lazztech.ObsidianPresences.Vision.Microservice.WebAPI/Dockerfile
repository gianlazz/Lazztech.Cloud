FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 50660
EXPOSE 44399

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["Lazztech.ObsidianPresences.Vision.Microservice.WebAPI/Lazztech.ObsidianPresences.Vision.Microservice.WebAPI.csproj", "Lazztech.ObsidianPresences.Vision.Microservice.WebAPI/"]
RUN dotnet restore "Lazztech.ObsidianPresences.Vision.Microservice.WebAPI/Lazztech.ObsidianPresences.Vision.Microservice.WebAPI.csproj"
COPY . .
WORKDIR "/src/Lazztech.ObsidianPresences.Vision.Microservice.WebAPI"
RUN dotnet build "Lazztech.ObsidianPresences.Vision.Microservice.WebAPI.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Lazztech.ObsidianPresences.Vision.Microservice.WebAPI.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Lazztech.ObsidianPresences.Vision.Microservice.WebAPI.dll"]