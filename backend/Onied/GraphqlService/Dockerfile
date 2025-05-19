FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["GraphqlService/GraphqlService.csproj", "GraphqlService/"]
COPY ["Courses/Courses.Data/Courses.Data.csproj", "Courses/Courses.Data/"]
RUN dotnet restore "GraphqlService/GraphqlService.csproj"
COPY . .
WORKDIR "/src/GraphqlService"
RUN dotnet build "./GraphqlService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./GraphqlService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GraphqlService.dll"]
