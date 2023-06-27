#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/Notification/Groceteria.NotificationMessgae.Processor/Groceteria.NotificationMessgae.Processor.csproj", "Services/Notification/Groceteria.NotificationMessgae.Processor/"]
COPY ["Services/Infrastructure/Groceteria.Infrastructure/Groceteria.Infrastructure.csproj", "Services/Infrastructure/Groceteria.Infrastructure/"]
COPY ["Services/Shared/Groceteria.Shared/Groceteria.Shared.csproj", "Services/Shared/Groceteria.Shared/"]
RUN dotnet restore "Services/Notification/Groceteria.NotificationMessgae.Processor/Groceteria.NotificationMessgae.Processor.csproj"
COPY . .
WORKDIR "/src/Services/Notification/Groceteria.NotificationMessgae.Processor"
RUN dotnet build "Groceteria.NotificationMessgae.Processor.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Groceteria.NotificationMessgae.Processor.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Groceteria.NotificationMessgae.Processor.dll"]