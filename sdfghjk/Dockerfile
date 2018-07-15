FROM microsoft/dotnet:2.1-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY Lazztech.ObsidianPresences.sln ./
COPY sdfghjk/sdfghjk.csproj sdfghjk/
RUN dotnet restore -nowarn:msb3202,nu1503
COPY . .
WORKDIR /src/sdfghjk
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "sdfghjk.dll"]
