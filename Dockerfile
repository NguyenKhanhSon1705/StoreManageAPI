# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY ["StoreManageAPI.csproj", "src/"]
RUN dotnet restore "src/StoreManageAPI.csproj"

# Copy the rest of the source code and build the project
COPY . . 
WORKDIR /src
RUN dotnet build "StoreManageAPI.csproj" -c Release -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "StoreManageAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app

# Copy the published files from the build stage
COPY --from=publish /app/publish .

# If you use a specific environment (e.g., Production), set the ASPNETCORE_ENVIRONMENT
ENV ASPNETCORE_ENVIRONMENT=Production

# Expose the port for the application
EXPOSE 80

# Specify the entry point for the application
ENTRYPOINT ["dotnet", "StoreManageAPI.dll"]
