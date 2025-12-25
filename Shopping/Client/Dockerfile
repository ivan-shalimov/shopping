FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

COPY ["Shopping/Client/Shopping.Client.csproj", "Shopping/Client/"]
COPY ["Shopping/Shared/Shopping.Shared.csproj", "Shopping/Shared/"]
RUN dotnet restore "Shopping/Client/Shopping.Client.csproj"
COPY . .
WORKDIR "/src/Shopping/Client"
RUN dotnet build Shopping.Client.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish Shopping.Client.csproj -c Release -o /app/publish

FROM nginx:alpine AS final

WORKDIR /usr/share/nginx/html

COPY --from=publish /app/publish/wwwroot .
COPY ./Shopping/Client/nginx.conf /etc/nginx/nginx.conf
COPY ./Shopping/Client/21-envsubst-on-appsettings-templates.sh /docker-entrypoint.d

EXPOSE 80