FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["srj.Blazor/srj.Blazor.csproj", "srj.Blazor/"]
COPY ["srj.Domain/srj.Domain.csproj", "srj.Domain/"]
COPY ["srj.Application/srj.Application.csproj", "srj.Application/"]
RUN dotnet restore "srj.Blazor/srj.Blazor.csproj"
COPY . .
WORKDIR "/src/srj.Blazor"
RUN dotnet build "./srj.Blazor.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./srj.Blazor.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "srj.Blazor.dll"]