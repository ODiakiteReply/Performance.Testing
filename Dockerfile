FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Performance.Testing.API/Performance.Testing.API.csproj", "Performance.Testing.API/"]
COPY ["src/Performance.Testing.Business/Performance.Testing.Business.csproj", "Performance.Testing.Business/"]
COPY ["src/Performance.Testing.Common/Performance.Testing.Common.csproj", "Performance.Testing.Common/"]
COPY ["src/Performance.Testing.DataAccess/Performance.Testing.DataAccess.csproj", "Performance.Testing.DataAccess/"]
COPY ["src/Performance.Testing.Models/Performance.Testing.Models.csproj", "Performance.Testing.Models/"]
COPY ["src/Performance.Testing.Translators/Performance.Testing.Translators.csproj", "Performance.Testing.Translators/"]
RUN dotnet restore "Performance.Testing.API/Performance.Testing.API.csproj"
COPY /src .
WORKDIR "/src/Performance.Testing.API"
RUN dotnet build "Performance.Testing.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Performance.Testing.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish ./

ENTRYPOINT ["dotnet", "Performance.Testing.API.dll"]

#ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false