﻿FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Notifications/Notifications/Notifications.csproj", "Notifications/Notifications/"]
COPY ["MassTransit.Data/MassTransit.Data.csproj", "MassTransit.Data/"]
COPY ["Common/Common.Validation/Common.Validation.csproj", "Common/Common.Validation/"]
COPY ["Notifications/Notifications.Data/Notifications.Data.csproj", "Notifications/Notifications.Data/"]
RUN dotnet restore "Notifications/Notifications/Notifications.csproj"
COPY . .
WORKDIR "/src/Notifications/Notifications"
RUN dotnet build "Notifications.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Notifications.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Notifications.dll"]
