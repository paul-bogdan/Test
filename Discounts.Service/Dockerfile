FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Discounts.Service/Discounts.Service.csproj", "Discounts.Service/"]
COPY ["Discounts.Domain/Discounts.Domain.csproj", "Discounts.Domain/"]
RUN dotnet restore "Discounts.Service/Discounts.Service.csproj"
COPY . .
WORKDIR "/src/Discounts.Service"
RUN dotnet build "Discounts.Service.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Discounts.Service.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Discounts.Service.dll"]
