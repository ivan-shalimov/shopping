#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Shopping/Server/Shopping.Server.csproj", "Shopping/Server/"]
COPY ["Shopping/Client/Shopping.Client.csproj", "Shopping/Client/"]
COPY ["Shopping/Shopping/Shopping.csproj", "Shopping/Shopping/"]
COPY ["Shopping/Shared/Shopping.Shared.csproj", "Shopping/Shared/"]
RUN dotnet restore "Shopping/Server/Shopping.Server.csproj"
COPY . .
WORKDIR "/src/Shopping/Server"
RUN dotnet build "Shopping.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Shopping.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Shopping.Server.dll"]