#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/Basket/Groceteria.Basket.Api/Groceteria.Basket.Api.csproj", "Services/Basket/Groceteria.Basket.Api/"]
COPY ["Services/infrastructure/Groceteria.Infrastructure/Groceteria.Infrastructure.csproj", "Services/infrastructure/Groceteria.Infrastructure/"]
COPY ["Services/shared/Groceteria.Shared/Groceteria.Shared.csproj", "Services/shared/Groceteria.Shared/"]
RUN dotnet restore "Services/Basket/Groceteria.Basket.Api/Groceteria.Basket.Api.csproj"
COPY . .
WORKDIR "/src/Services/Basket/Groceteria.Basket.Api"
RUN dotnet build "Groceteria.Basket.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Groceteria.Basket.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Groceteria.Basket.Api.dll"]