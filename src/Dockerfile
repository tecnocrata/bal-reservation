# Stage 1: Base image with runtime dependencies
FROM --platform=linux/amd64 mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

# Stage 2: Build image with SDK
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
# ARG app_dir=./app
WORKDIR /app
# main project
COPY ["Api/Api.csproj", "Api/"]
COPY ["nuget.config", "Api/nuget.config"]

# Copy csproj files of additional projects
COPY ["Modules/Reservations/", "Modules/Reservations/"]
COPY ["Modules/SystemUsers/", "Modules/SystemUsers/"]
COPY ["Shared/", "Shared/"]
# COPY ["Shared/Shared/", "Shared/Shared/"]

RUN dotnet nuget locals all --clear && dotnet restore "Api/Api.csproj" --configfile Api/nuget.config -v d

# Copy everything else and build
COPY . .
# WORKDIR /src/Api
RUN dotnet build "Api/Api.csproj" -c Release -o /app/build

# Stage 3: Publish image
FROM build AS publish
# WORKDIR /src/Api
RUN dotnet publish "Api/Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 4: Final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .



ENTRYPOINT ["dotnet", "Api.dll"]