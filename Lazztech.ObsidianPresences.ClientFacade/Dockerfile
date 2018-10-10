FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 50472
EXPOSE 44362

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["Lazztech.ObsidianPresences.ClientFacade/Lazztech.ObsidianPresences.ClientFacade.csproj", "Lazztech.ObsidianPresences.ClientFacade/"]
RUN dotnet restore "Lazztech.ObsidianPresences.ClientFacade/Lazztech.ObsidianPresences.ClientFacade.csproj"
COPY . .
WORKDIR "/src/Lazztech.ObsidianPresences.ClientFacade"
RUN dotnet build "Lazztech.ObsidianPresences.ClientFacade.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Lazztech.ObsidianPresences.ClientFacade.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Lazztech.ObsidianPresences.ClientFacade.dll"]