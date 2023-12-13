# Build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copy the main solution file and restore as distinct layers
COPY ["x_service.NoTest.sln", "./"]

# Copy the main project file and restore as distinct layers
COPY ["src/x_serviceAPI/x_serviceAPI.csproj", "src/x_serviceAPI/"]
RUN dotnet restore

# Copy everything else and build
COPY . .
RUN dotnet publish "src/x_serviceAPI/x_serviceAPI.csproj" -c Release -o out

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "x_serviceAPI.dll"]