# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia os arquivos csproj
COPY backend/ProductService/ProductService.csproj ProductService/
COPY backend/Ecommerce/Ecommerce.csproj Ecommerce/

# Restaura dependências
RUN dotnet restore ProductService/ProductService.csproj

# Copia todo o código
COPY backend/ProductService ./ProductService
COPY backend/Ecommerce ./Ecommerce

# Publica
WORKDIR /src/ProductService
RUN dotnet publish -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copia os arquivos publicados
COPY --from=build /app/publish .

# Expor a porta
EXPOSE 5000

# Comando para iniciar
ENTRYPOINT ["dotnet", "ProductService.dll"]
