FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Courses/Courses/Courses.csproj", "Courses/Courses/"]
COPY ["Courses/Courses.Data/Courses.Data.csproj", "Courses/Courses.Data/"]
COPY ["MassTransit.Data/MassTransit.Data.csproj", "MassTransit.Data/"]
COPY ["Common/Common.Validation/Common.Validation.csproj", "Common/Common.Validation/"]
COPY ["Shared/Shared.csproj", "Shared/"]
RUN dotnet restore "Courses/Courses/Courses.csproj"
COPY . .
WORKDIR "/src/Courses/Courses"
RUN dotnet build "Courses.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Courses.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Courses.dll"]
