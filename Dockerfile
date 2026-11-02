# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["CoreConcepts.csproj", "./"]
RUN dotnet restore "./CoreConcepts.csproj"

COPY . .
RUN dotnet publish "./CoreConcepts.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

COPY --from=build /app/publish .

# Create uploads directories
RUN mkdir -p /app/wwwroot/uploads/resumes /app/wwwroot/uploads/leaders /app/wwwroot/uploads/banners /app/wwwroot/uploads/projects /app/wwwroot/uploads/cards

ENTRYPOINT ["dotnet", "CoreConcepts.dll"]
