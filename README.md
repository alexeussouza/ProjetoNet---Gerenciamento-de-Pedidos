# 📦 Sistema de Pedidos - E-commerce B2C

Projeto de um sistema de pedidos para e-commerce B2C, desenvolvido como parte de prova técnica. O sistema é composto por backend em .NET, frontend em React + TypeScript, banco de dados PostgreSQL e orquestração via Docker Compose.

---

## 🚀 Tecnologias Utilizadas

- ⚙️ **Backend**: C# (.NET 8)
- 🌐 **Frontend**: React + TypeScript (Vite)
- 🐘 **Banco de Dados**: PostgreSQL (Docker)
- 🔒 **Autenticação**: JWT
- 🐳 **Containerização**: Docker e Docker Compose
- 📦 **NGINX como API Gateway**
- 🗂️ **Biblioteca Compartilhada**: Ecommerce (Models, DTOs, Enums)
- ✅ **API RESTful**

---

## 📜 Funcionalidades

- 🔐 Registro e login com JWT
- 🛍️ Listagem de produtos
- 📦 Criação de pedidos
- 💳 Simulação de pagamento (80% sucesso, 20% falha)
- 🏪 Reserva de estoque
- 📑 Listagem e consulta de pedidos
- 🔒 Endpoints protegidos via JWT
- ✅ Validação de entradas e retorno de códigos HTTP apropriados

---

## ⚙️ Como Executar o Projeto

### 🔥 1. Clonar o repositório

```bash
git clone https://github.com/SENAI-SD/prova-csharp-pleno-01411-2025-722.643.141-68.git
cd prova-csharp-pleno-01411-2025-722.643.141-68

### 🐳 2. Subir os containers
Certifique-se de ter Docker e Docker Compose instalados.

bash
docker compose up -d --build
Esse comando irá:

Construir imagens do AuthService, OrderService, ProductService e Frontend.

Subir containers de PostgreSQL e NGINX.

Mapear corretamente as portas e volumes necessários.

> 💡 O arquivo nginx.conf é montado no container como volume em /etc/nginx/nginx.conf.

### 🚀 3. Backend (.NET)
As migrations são aplicadas automaticamente.

Se necessário, rode manualmente:

bash
docker exec -it authservice dotnet ef database update
### 🌐 4. Frontend (React + Vite)
bash
cd frontend
npm install
npm run dev
Por padrão:

localhost:5173 (modo dev)

localhost (via NGINX no container)

---
### 📂 Estrutura do Projeto

```
prova-csharp-pleno-01411-2025-722.643.141-68/
├── backend/
│   ├── AuthService/
│   ├── OrderService/
│   ├── ProductService/
│   └── Ecommerce/       # Biblioteca compartilhada
├── frontend/
├── nginx/
│   └── nginx.conf
├── docker-compose.yml
└── README.md
```
---

##🔧 Configuração do NGINX (nginx.conf)
### ✅ Objetivo
Servir como gateway reverso.

Interceptar requisições OPTIONS com CORS válido (pré-flight).

Repassar POST, GET e outros métodos para os containers corretos.

Evitar problemas de rewrite e conflitos com o backend.

### ✨ Exemplo de bloco funcional (login)
nginx
location ^~ /api/auth/login {
    if ($request_method = OPTIONS) {
        add_header 'Access-Control-Allow-Origin' '*' always;
        add_header 'Access-Control-Allow-Methods' 'POST, OPTIONS' always;
        add_header 'Access-Control-Allow-Headers' 'Authorization, Content-Type' always;
        add_header 'Access-Control-Max-Age' 1728000 always;
        add_header 'Content-Length' 0;
        add_header 'Content-Type' 'text/plain; charset=UTF-8';
        return 204;
    }

    proxy_pass http://authservice;
    proxy_set_header Host $host;
    proxy_set_header X-Real-IP $remote_addr;
    proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
    proxy_set_header X-Forwarded-Proto $scheme;
}

### 📌 Demais rotas
/api/orders/ → orderservice

/api/products/ → productservice

/api/auth/* genérico → authservice

---

## 🔐 JWT - Segurança
✔ Requisitos
Algoritmo HS256

Tamanho mínimo da chave secreta: 256 bits / 32 caracteres

Exemplo válido:

json
"JwtSettings": {
  "Key": "super-secret-key-at-least-32-characters!"
}
Evita erros como:

IDX10720: key size must be greater than 256 bits

---

## 🧪 Testes de Comunicação
### ✅ Registro
bash
curl -X POST http://localhost:5000/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{"name":"Alex","email":"alex@teste.com","password":"123456"}'
### ✅ Login
bash
curl -X POST http://localhost:5000/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"alex@teste.com","password":"123456"}'
### ✅ Produtos
bash
curl -H "Authorization: Bearer SEU_TOKEN" http://localhost:5002/api/products

###📝 Comandos Úteis
bash
docker compose down                          # Parar e remover containers
docker compose build --no-cache              # Build do zero
docker ps                                    # Ver containers ativos
docker logs authservice                      # Ver logs do backend
docker exec -it frontend ping authservice    # Testar comunicação interna

---

## 🔒 Observações Técnicas
- ✅ Node.js: versão mínima recomendada: 18+

- ✅ .NET 8 via SDK Docker

- ✅ CORS no .NET:

csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

app.UseCors("AllowAll");

--- 

## 🧠 Contribuição
bash
git checkout -b feature/nova-feature
git commit -m "feat: nova feature"
git push origin feature/nova-feature
Abra um Pull Request para a branch dev.

---

## 🏆 Autor
Desenvolvido por Alexandre de Souza Eustáquio

---

## 📜 Licença
Este projeto está sob licença MIT.
