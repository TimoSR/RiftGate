# Build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app


# Copy the solution file
COPY ["x_service.NoTest.sln", "./"]

# Copy shared libraries
COPY ["src/_SharedLibraries/Riftgate-Microservice-Core/Infrastructure/Infrastructure.csproj", "src/_SharedLibraries/Riftgate-Microservice-Core/Infrastructure/"]
COPY ["src/_SharedLibraries/Riftgate-Microservice-Core/CodingPatterns/CodingPatterns.csproj", "src/_SharedLibraries/Riftgate-Microservice-Core/CodingPatterns/"]

# Copy csproj and restore
COPY ["src/API/API.csproj", "src/API/"]
RUN dotnet restore

# Copy everything else and build
COPY . .
RUN dotnet publish "src/API/API.csproj" -c Release -o ou

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "API.dll"]