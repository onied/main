FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Courses/Courses.csproj", "Courses/"]
COPY ["MassTransit.Data/MassTransit.Data.csproj", "MassTransit.Data/"]
COPY ["Shared/Shared.csproj", "Shared/"]
RUN dotnet restore "Courses/Courses.csproj"
COPY . .
WORKDIR "/src/Courses"
RUN dotnet build "Courses.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Courses.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Courses.dll"]
