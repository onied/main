FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Purchases/Purchases.csproj", "Purchases/"]
COPY ["Purchases.Data/Purchases.Data.csproj", "Purchases.Data/"]
COPY ["MassTransit.Data/MassTransit.Data.csproj", "MassTransit.Data/"]
RUN dotnet restore "Purchases/Purchases.csproj"
COPY . .
WORKDIR "/src/Purchases"
RUN dotnet build "Purchases.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Purchases.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Purchases.dll"]
