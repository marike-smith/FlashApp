FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["FlashApp.IdentityHost/FlashApp.IdentityHost.csproj", "FlashApp.IdentityHost/"]
RUN dotnet restore "FlashApp.IdentityHost/FlashApp.IdentityHost.csproj"
COPY . .
WORKDIR "/src/FlashApp.IdentityHost"
RUN dotnet build "FlashApp.IdentityHost.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "FlashApp.IdentityHost.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FlashApp.IdentityHost.dll"]
