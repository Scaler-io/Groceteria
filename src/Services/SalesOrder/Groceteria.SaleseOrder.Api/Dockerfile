#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/SalesOrder/Groceteria.SaleseOrder.Api/Groceteria.SaleseOrder.Api.csproj", "Services/SalesOrder/Groceteria.SaleseOrder.Api/"]
COPY ["Services/Infrastructure/Groceteria.Infrastructure/Groceteria.Infrastructure.csproj", "Services/Infrastructure/Groceteria.Infrastructure/"]
COPY ["Services/Shared/Groceteria.Shared/Groceteria.Shared.csproj", "Services/Shared/Groceteria.Shared/"]
COPY ["Services/SalesOrder/Groceteria.SalesOrder.Application/Groceteria.SalesOrder.Application.csproj", "Services/SalesOrder/Groceteria.SalesOrder.Application/"]
COPY ["Services/SalesOrder/Groceteria.SalesOrder.Domain/Groceteria.SalesOrder.Domain.csproj", "Services/SalesOrder/Groceteria.SalesOrder.Domain/"]
COPY ["Services/SalesOrder/Groceteria.SalesOrder.Infrastructure/Groceteria.SalesOrder.Infrastructure.csproj", "Services/SalesOrder/Groceteria.SalesOrder.Infrastructure/"]
COPY ["Services/SalesOrder/Swagger/Swagger.csproj", "Services/SalesOrder/Swagger/"]
RUN dotnet restore "Services/SalesOrder/Groceteria.SaleseOrder.Api/Groceteria.SaleseOrder.Api.csproj"
COPY . .
WORKDIR "/src/Services/SalesOrder/Groceteria.SaleseOrder.Api"
RUN dotnet build "Groceteria.SaleseOrder.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Groceteria.SaleseOrder.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Groceteria.SaleseOrder.Api.dll"]