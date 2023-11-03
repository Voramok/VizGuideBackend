FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["VizGuideBackend/VizGuideBackend.csproj", "VizGuideBackend/"]
RUN dotnet restore "VizGuideBackend/VizGuideBackend.csproj"
COPY . .
WORKDIR "/src/VizGuideBackend"
RUN dotnet build "VizGuideBackend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "VizGuideBackend.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VizGuideBackend.dll"]