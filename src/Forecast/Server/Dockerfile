#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Forecast/Server/Forecast.Server.csproj", "src/Forecast/Server/"]
COPY ["src/Forecast/Client/Forecast.Client.csproj", "src/Forecast/Client/"]
COPY ["src/Forecast/Shared/Forecast.Shared.csproj", "src/Forecast/Shared/"]
RUN dotnet restore "./src/Forecast/Server/./Forecast.Server.csproj"
COPY . .
WORKDIR "/src/src/Forecast/Server"
RUN dotnet build "./Forecast.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Forecast.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Forecast.Server.dll"]