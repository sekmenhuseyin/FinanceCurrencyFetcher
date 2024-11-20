FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
ENV TZ=Europe/Istanbul
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["CachedFinanceData/CachedFinanceData.csproj", "CachedFinanceData/"]
RUN dotnet restore "CachedFinanceData/CachedFinanceData.csproj"

COPY . .
WORKDIR "/src/CachedFinanceData"
RUN dotnet build "CachedFinanceData.csproj" -c "$BUILD_CONFIGURATION" -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "CachedFinanceData.csproj" -c "$BUILD_CONFIGURATION" -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "CachedFinanceData.dll"]
