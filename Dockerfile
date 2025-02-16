#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["src/FlashApp.API/FlashApp.API.csproj", "src/FlashApp.API/"]
RUN dotnet restore "src/FlashApp.API/FlashApp.API.csproj"
COPY . .
WORKDIR "/src/src/FlashApp.API"
RUN dotnet build "FlashApp.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FlashApp.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FlashApp.API.dll"]