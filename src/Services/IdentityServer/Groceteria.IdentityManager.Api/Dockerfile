#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Install networking tools
RUN apt-get update && apt-get install -y iputils-ping dnsutils curl

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/IdentityServer/Groceteria.IdentityManager.Api/Groceteria.IdentityManager.Api.csproj", "Services/IdentityServer/Groceteria.IdentityManager.Api/"]
COPY ["Services/IdentityServer/GrOceteria.Identity.Shared/Groceteria.Identity.Shared.csproj", "Services/IdentityServer/GrOceteria.Identity.Shared/"]
RUN dotnet restore "Services/IdentityServer/Groceteria.IdentityManager.Api/Groceteria.IdentityManager.Api.csproj"
COPY . .
WORKDIR "/src/Services/IdentityServer/Groceteria.IdentityManager.Api"
RUN dotnet build "Groceteria.IdentityManager.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Groceteria.IdentityManager.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Groceteria.IdentityManager.Api.dll"]