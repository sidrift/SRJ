FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["srj.Api/srj.Api.csproj", "srj.Api/"]
COPY ["srj.Application/srj.Application.csproj", "srj.Application/"]
COPY ["srj.Domain/srj.Domain.csproj", "srj.Domain/"]
COPY ["srj.Infrastructure/srj.Infrastructure.csproj", "srj.Infrastructure/"]
RUN dotnet restore "srj.Api/srj.Api.csproj"
COPY . .
WORKDIR "/src/srj.Api"
RUN dotnet build "./srj.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./srj.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "srj.Api.dll"]