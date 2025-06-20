# Etapa 1: Construção do aplicativo (React)
FROM node:20 AS build

# Definir o diretório de trabalho
WORKDIR /app

# Copiar apenas os arquivos do projeto (não copie node_modules)
COPY package*.json ./

# Instalar dependências
RUN npm install

# Copiar o restante dos arquivos
COPY . .

# Gerar a build
RUN npm run build

# Etapa 2: Servir os arquivos com o Nginx
FROM nginx:alpine

# Definir o diretório de trabalho do Nginx
WORKDIR /usr/share/nginx/html

# Remover os arquivos padrão do Nginx
RUN rm -rf ./*

# Copiar os arquivos gerados na etapa de build para o diretório do Nginx
COPY --from=build /app/dist .  

# Copiar o arquivo de configuração do Nginx (se necessário)
COPY ./nginx/default.conf /etc/nginx/conf.d/default.conf

# Expor a porta 80
EXPOSE 80

# Comando para rodar o Nginx
CMD ["nginx", "-g", "daemon off;"]