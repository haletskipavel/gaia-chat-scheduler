# Use the official .NET 8.0 SDK as the base image for building the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy project files and restore dependencies
COPY ["Airdrops.Nodes.Scheduler/Airdrops.Nodes.Scheduler.csproj", "Airdrops.Nodes.Scheduler/"]
COPY ["Airdrops.Nodes.Domain/Airdrops.Nodes.Domain.csproj", "Airdrops.Nodes.Domain/"]
COPY ["Airdrops.Nodes.Infrastructure/Airdrops.Nodes.Infrastructure.csproj", "Airdrops.Nodes.Infrastructure/"]
RUN dotnet restore "Airdrops.Nodes.Scheduler/Airdrops.Nodes.Scheduler.csproj"

# Copy the remaining source code and build the application
COPY . .
RUN dotnet build "Airdrops.Nodes.Scheduler/Airdrops.Nodes.Scheduler.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the application to a publish directory
RUN dotnet publish "Airdrops.Nodes.Scheduler/Airdrops.Nodes.Scheduler.csproj" -c $BUILD_CONFIGURATION -o /app/publish

# Use the official .NET 8.0 runtime as the base image for running the app
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS final
WORKDIR /app

# Copy the published app from the build stage
COPY --from=build /app/publish .

# Run the application
ENTRYPOINT ["dotnet", "Airdrops.Nodes.Scheduler.dll"]