#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/catalogue/Groceteria.Catalogue.Api/Groceteria.Catalogue.Api.csproj", "Services/catalogue/Groceteria.Catalogue.Api/"]
COPY ["Services/infrastructure/Groceteria.Infrastructure/Groceteria.Infrastructure.csproj", "Services/infrastructure/Groceteria.Infrastructure/"]
COPY ["Services/shared/Groceteria.Shared/Groceteria.Shared.csproj", "Services/shared/Groceteria.Shared/"]
RUN dotnet restore "Services/catalogue/Groceteria.Catalogue.Api/Groceteria.Catalogue.Api.csproj"
COPY . .
WORKDIR "/src/Services/catalogue/Groceteria.Catalogue.Api"
RUN dotnet build "Groceteria.Catalogue.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Groceteria.Catalogue.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Groceteria.Catalogue.Api.dll"]