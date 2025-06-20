# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia os arquivos csproj
COPY backend/OrderService/OrderService.csproj OrderService/
COPY backend/Ecommerce/Ecommerce.csproj Ecommerce/

# Restaura dependências
RUN dotnet restore OrderService/OrderService.csproj

# Copia todo o código
COPY backend/OrderService ./OrderService
COPY backend/Ecommerce ./Ecommerce

# Publica
WORKDIR /src/OrderService
RUN dotnet publish -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copia os arquivos publicados
COPY --from=build /app/publish .

# Expor a porta
EXPOSE 5000

# Comando para iniciar
ENTRYPOINT ["dotnet", "OrderService.dll"]

