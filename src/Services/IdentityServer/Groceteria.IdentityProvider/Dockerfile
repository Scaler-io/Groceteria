#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/IdentityServer/Groceteria.IdentityProvider/Groceteria.IdentityProvider.csproj", "Services/IdentityServer/Groceteria.IdentityProvider/"]
COPY ["Services/IdentityServer/GrOceteria.Identity.Shared/Groceteria.Identity.Shared.csproj", "Services/IdentityServer/GrOceteria.Identity.Shared/"]
RUN dotnet restore "Services/IdentityServer/Groceteria.IdentityProvider/Groceteria.IdentityProvider.csproj"
COPY . .
WORKDIR "/src/Services/IdentityServer/Groceteria.IdentityProvider"
RUN dotnet build "Groceteria.IdentityProvider.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Groceteria.IdentityProvider.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Groceteria.IdentityProvider.dll"]