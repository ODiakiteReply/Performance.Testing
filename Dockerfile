# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
# For more information, please see https://aka.ms/containercompat

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Performance.Testing.API/Performance.Testing.API.csproj", "src/Performance.Testing.API/"]
COPY ["src/Performance.Testing.Business/Performance.Testing.Business.csproj", "src/Performance.Testing.Business/"]
COPY ["src/Performance.Testing.Common/Performance.Testing.Common.csproj", "src/Performance.Testing.Common/"]
COPY ["src/Performance.Testing.DataAccess/Performance.Testing.DataAccess.csproj", "src/Performance.Testing.DataAccess/"]
COPY ["src/Performance.Testing.Models/Performance.Testing.Models.csproj", "src/Performance.Testing.Models/"]
COPY ["src/Performance.Testing.Translators/Performance.Testing.Translators.csproj", "src/Performance.Testing.Translators/"]
RUN dotnet restore "src/Performance.Testing.API/Performance.Testing.API.csproj"
COPY . .
WORKDIR "/src/src/Performance.Testing.API"
RUN dotnet build "Performance.Testing.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Performance.Testing.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Performance.Testing.API.dll"]