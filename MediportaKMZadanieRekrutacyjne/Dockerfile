#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

#FROM mcr.microsoft.com/dotnet/aspnet:8.0-nanoserver-1809 AS base
#WORKDIR /app
#EXPOSE 8080
#EXPOSE 8081
#
#FROM mcr.microsoft.com/dotnet/sdk:8.0-nanoserver-1809 AS build
#ARG BUILD_CONFIGURATION=Release
#WORKDIR /src
#COPY ["MediportaKMZadanieRekrutacyjne/MediportaKMZadanieRekrutacyjne.csproj", "MediportaKMZadanieRekrutacyjne/"]
#
#RUN dotnet restore "./MediportaKMZadanieRekrutacyjne/MediportaKMZadanieRekrutacyjne.csproj"
#COPY . .
#WORKDIR "/src/MediportaKMZadanieRekrutacyjne"
#RUN dotnet build "./MediportaKMZadanieRekrutacyjne.csproj" -c %BUILD_CONFIGURATION% -o /app/build
#
#FROM build AS publish
#ARG BUILD_CONFIGURATION=Release
#RUN dotnet publish "./MediportaKMZadanieRekrutacyjne.csproj" -c %BUILD_CONFIGURATION% -o /app/publish /p:UseAppHost=false
#
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "MediportaKMZadanieRekrutacyjne.dll"]

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Kopiowanie csproj i przywracanie zale¿noœci
COPY *.csproj ./
RUN dotnet restore

# Kopiowanie wszystkiego i budowanie
COPY . ./
RUN dotnet publish -c Release -o out

# Budowanie obrazu runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "MediportaKMZadanieRekrutacyjne.dll"]
