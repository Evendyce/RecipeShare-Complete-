# ----------------------------------------
# Base SDK Image for Build
# ----------------------------------------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy project files only for restore first (better layer caching)
COPY RecipeShare.Models/RecipeShare.Models.csproj ./RecipeShare.Models/
COPY RecipeShare.Utils/RecipeShare.Utils.csproj ./RecipeShare.Utils/
COPY RecipeShare.API/RecipeShare.API.csproj ./RecipeShare.API/

# Restore dependencies
RUN dotnet restore ./RecipeShare.API/RecipeShare.API.csproj

# Now copy everything else (source files, assets, etc.)
COPY RecipeShare.Models/ ./RecipeShare.Models/
COPY RecipeShare.Utils/ ./RecipeShare.Utils/
COPY RecipeShare.API/ ./RecipeShare.API/

# Build
WORKDIR /src/RecipeShare.API
RUN dotnet build "RecipeShare.API.csproj" -c Release -o /app/build \
    /p:RestoreFallbackFolders="" \
    /p:BuildingInsideDocker=true

# ----------------------------------------
# Final runtime image
# ----------------------------------------
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Copy the built app
COPY --from=build /app/build .

# Use port 80 by default (adjust as needed)
EXPOSE 80
ENV ASPNETCORE_URLS=http://+:80

ENTRYPOINT ["dotnet", "RecipeShare.API.dll"]
