FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
RUN apt-get -y update; apt-get -y install curl
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Users/Users/Users.csproj", "Users/Users/"]
COPY ["MassTransit.Data/MassTransit.Data.csproj", "MassTransit.Data/"]
COPY ["Common/Common.Validation/Common.Validation.csproj", "Common/Common.Validation/"]
COPY ["Users/Users.Data/Users.Data.csproj", "Users/Users.Data/"]
COPY ["Shared/Shared.csproj", "Shared/"]
RUN dotnet restore "Users/Users/Users.csproj"
COPY . .
RUN mv "Users/Users/ocelot.Docker.json" "Users/Users/ocelot.json"
WORKDIR "/src/Users/Users"
RUN dotnet build "Users.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Users.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Users.dll"]
