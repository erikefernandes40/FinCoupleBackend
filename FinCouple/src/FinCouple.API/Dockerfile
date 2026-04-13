FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/FinCouple.API/FinCouple.API.csproj", "src/FinCouple.API/"]
COPY ["src/FinCouple.Application/FinCouple.Application.csproj", "src/FinCouple.Application/"]
COPY ["src/FinCouple.Infrastructure/FinCouple.Infrastructure.csproj", "src/FinCouple.Infrastructure/"]
COPY ["src/FinCouple.Domain/FinCouple.Domain.csproj", "src/FinCouple.Domain/"]
RUN dotnet restore "src/FinCouple.API/FinCouple.API.csproj"
COPY . .
WORKDIR "/src/src/FinCouple.API"
RUN dotnet build "FinCouple.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FinCouple.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FinCouple.API.dll"]
