# Stage 1: Build 
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar os arquivos csproj para restaurar dependências
COPY backend/AuthService/AuthService.csproj ./AuthService/
COPY backend/Ecommerce/Ecommerce.csproj ./Ecommerce/

# Restaurar as dependências
RUN dotnet restore ./AuthService/AuthService.csproj

# Copiar todo o código-fonte para a imagem
COPY backend/AuthService ./AuthService
COPY backend/Ecommerce ./Ecommerce

# Compilar e publicar a aplicação
WORKDIR /src/AuthService
RUN dotnet publish -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copiar os arquivos publicados do build
COPY --from=build /app/publish .

# Copiar o arquivo de configuração appsettings.json
COPY backend/AuthService/appsettings.json .

ENV ASPNETCORE_URLS=http://+:8080

EXPOSE 8080

ENTRYPOINT ["dotnet", "AuthService.dll"]