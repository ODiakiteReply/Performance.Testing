FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/AZWebAppCDB1.API/AZWebAppCDB1.API.csproj", "AZWebAppCDB1.API/"]
COPY ["src/AZWebAppCDB1.Business/AZWebAppCDB1.Business.csproj", "AZWebAppCDB1.Business/"]
COPY ["src/AZWebAppCDB1.Common/AZWebAppCDB1.Common.csproj", "AZWebAppCDB1.Common/"]
COPY ["src/AZWebAppCDB1.DataAccess/AZWebAppCDB1.DataAccess.csproj", "AZWebAppCDB1.DataAccess/"]
COPY ["src/AZWebAppCDB1.Models/AZWebAppCDB1.Models.csproj", "AZWebAppCDB1.Models/"]
COPY ["src/AZWebAppCDB1.Translators/AZWebAppCDB1.Translators.csproj", "AZWebAppCDB1.Translators/"]
RUN dotnet restore "AZWebAppCDB1.API/AZWebAppCDB1.API.csproj"
COPY /src .
WORKDIR "/src/AZWebAppCDB1.API"
RUN dotnet build "AZWebAppCDB1.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AZWebAppCDB1.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish ./

ENTRYPOINT ["dotnet", "AZWebAppCDB1.API.dll"]

#ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false