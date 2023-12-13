# Use the SDK image to build the projects
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copy the solution file
COPY ["x_service.NoTest.sln", "./"]

# Copy the main API project - adjust the path as necessary
COPY ["src/API/API.csproj", "./src/API/"]

# Copy the shared library projects - adjust the path as necessary
COPY ["_SharedLibraries/Riftgate-Microservice-Core/CodingPatterns/CodingPatterns.csproj", "./_SharedLibraries/Riftgate-Microservice-Core/CodingPatterns/"]
COPY ["_SharedLibraries/Riftgate-Microservice-Core/Infrastructure/Infrastructure.csproj", "./_SharedLibraries/Riftgate-Microservice-Core/Infrastructure/"]

# Restore the packages
RUN dotnet restore "x_service.NoTest.sln"

# Copy the remaining source files and build the application
COPY . .
RUN dotnet publish "src/API/API.csproj" -c Release -o out

# Use the runtime image to run the app
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "API.dll"]
