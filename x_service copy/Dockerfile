# Use the SDK image to build the projects
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copy the solution file
COPY ["x_service.NoTest.sln", "./"]

# Copy the main API project and its dependencies
COPY ["src/API/API.csproj", "src/API/"]
COPY ["src/RiftGate-Microservice-Core/CodingPatterns/CodingPatterns.csproj", "src/RiftGate-Microservice-Core/CodingPatterns/"]
COPY ["src/RiftGate-Microservice-Core/Infrastructure/Infrastructure.csproj", "src/RiftGate-Microservice-Core/Infrastructure/"]

# Restore the packages
RUN dotnet restore "x_service.NoTest.sln"

# Copy the source code
COPY ["src/", "src/"]

RUN dotnet publish "src/API/API.csproj" -c Release -o out

# Use the runtime image to run the app
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "API.dll"]
