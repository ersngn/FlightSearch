FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/FlightSearchAPI.API/FlightSearchAPI.API.csproj", "src/FlightSearchAPI.API/"]
RUN dotnet restore "src/FlightSearchAPI.API/FlightSearchAPI.API.csproj"
COPY . .
WORKDIR "/src/src/FlightSearchAPI.API"
RUN dotnet build "FlightSearchAPI.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FlightSearchAPI.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FlightSearchAPI.API.dll"]
