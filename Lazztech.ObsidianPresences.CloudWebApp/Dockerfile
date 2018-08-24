FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
ENV ASPNETCORE_URLS=http://+:5000
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
ENV ASPNETCORE_URLS=http://+:5000
WORKDIR /src
COPY Lazztech.ObsidianPresences.sln ./
COPY Lazztech.ObsidianPresences.CloudWebApp/Lazztech.ObsidianPresences.CloudWebApp.csproj Lazztech.ObsidianPresences.CloudWebApp/
RUN dotnet restore -nowarn:msb3202,nu1503
COPY . .
WORKDIR /src/Lazztech.ObsidianPresences.CloudWebApp
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet watch", "Lazztech.ObsidianPresences.CloudWebApp.dll"]
