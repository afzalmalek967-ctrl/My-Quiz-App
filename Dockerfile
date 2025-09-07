# Stage 1: Build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copy all files and restore
COPY . ./
RUN dotnet restore

# Publish the app
RUN dotnet publish -c Release -o out

# Stage 2: Serve the app
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "QuizApp.dll"]