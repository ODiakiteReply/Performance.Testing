FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
WORKDIR /app
COPY /dist .

ENTRYPOINT ["dotnet", "AZWebAppCDB1.API.dll"]

RUN apk add --no-cache icu-libs

#installs libgdiplus to support System.Drawing for handling of graphics
RUN apk add --no-cache libgdiplus --repository http://dl-cdn.alpinelinux.org/alpine/edge/testing/

#installs some standard fonts needed for Autofit columns support
RUN apk --no-cache add msttcorefonts-installer fontconfig freetype-dev libjpeg-turbo-dev libpng-dev && \
    update-ms-fonts && \
    fc-cache -f

ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false