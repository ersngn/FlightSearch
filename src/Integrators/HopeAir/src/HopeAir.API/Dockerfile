FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/Integrators/HopeAir/src/HopeAir.API/HopeAir.API.csproj", "src/Integrators/HopeAir/src/HopeAir.API/"]
RUN dotnet restore "src/Integrators/HopeAir/src/HopeAir.API/HopeAir.API.csproj"
COPY . .
WORKDIR "/src/src/Integrators/HopeAir/src/HopeAir.API"
RUN dotnet build "HopeAir.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HopeAir.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HopeAir.API.dll"]
