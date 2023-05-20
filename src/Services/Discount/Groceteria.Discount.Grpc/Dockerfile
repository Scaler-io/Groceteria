#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/Discount/Groceteria.Discount.Grpc/Groceteria.Discount.Grpc.csproj", "Services/Discount/Groceteria.Discount.Grpc/"]
COPY ["Services/Infrastructure/Groceteria.Infrastructure/Groceteria.Infrastructure.csproj", "Services/Infrastructure/Groceteria.Infrastructure/"]
COPY ["Services/Shared/Groceteria.Shared/Groceteria.Shared.csproj", "Services/Shared/Groceteria.Shared/"]
RUN dotnet restore "Services/Discount/Groceteria.Discount.Grpc/Groceteria.Discount.Grpc.csproj"
COPY . .
WORKDIR "/src/Services/Discount/Groceteria.Discount.Grpc"
RUN dotnet build "Groceteria.Discount.Grpc.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Groceteria.Discount.Grpc.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Groceteria.Discount.Grpc.dll"]