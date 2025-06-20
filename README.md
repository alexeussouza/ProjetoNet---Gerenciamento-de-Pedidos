# 📦 Sistema de Pedidos - E-commerce B2C

Projeto de um sistema de pedidos para e-commerce B2C, desenvolvido como parte de prova técnica. O sistema é composto por backend em .NET, frontend em React + TypeScript, banco de dados PostgreSQL e orquestração via Docker Compose.

## 🚀 Tecnologias Utilizadas

- ⚙️ Backend: C# (.NET 8)
- 🌐 Frontend: React + TypeScript (Vite)
- 🐘 Banco de Dados: PostgreSQL (Docker)
- 🔒 Autenticação: JWT
- 🐳 Containerização: Docker e Docker Compose
- 📦 NGINX como API Gateway
- 🗂️ Biblioteca Compartilhada: Ecommerce (Models, DTOs, Enums)
- ✅ API RESTful

## 📜 Funcionalidades

- 🔐 Registro e login com JWT
- 🛍️ Listagem de produtos
- 📦 Criação de pedidos
- 💳 Simulação de pagamento (80% sucesso, 20% falha)
- 🏪 Reserva de estoque
- 📑 Listagem e consulta de pedidos
- 🔒 Endpoints protegidos
- ✅ Validação de entradas e retorno de códigos HTTP apropriados

## ⚙️ Como Executar o Projeto

### 🔥 1. Clonar o repositório

```bash
git clone https://github.com/SENAI-SD/prova-csharp-pleno-01411-2025-722.643.141-68.git
cd prova-csharp-pleno-01411-2025-722.643.141-68
```

### 🐳 2. Subir os containers
Certifique-se de ter Docker e Docker Compose instalados.

```bash
docker compose up -d --build
```

Este comando irá:
- Construir imagens do AuthService, OrderService, ProductService e Frontend.
- Subir containers de PostgreSQL e NGINX.
- Mapear corretamente as portas.

### 🚀 3. Backend (.NET)
As migrations já são aplicadas automaticamente via Docker Compose.

Se desejar rodar manualmente:

```bash
docker exec -it authservice dotnet ef database update
```

### 🌐 4. Frontend (React + Vite)

#### Instalar dependências

Acesse a pasta `frontend/` e execute:

```bash
npm install
```

#### Iniciar manualmente (se quiser rodar local sem docker)

```bash
npm run dev
```

Por padrão, estará disponível em:

[http://localhost:5173](http://localhost:5173)

Obs: Pelo Docker Compose, o frontend já é servido via NGINX em [http://localhost](http://localhost)

## 📂 Estrutura do Projeto

```
prova-csharp-pleno-01411-2025-722.643.141-68/
├── backend/
│   ├── AuthService/
│   ├── OrderService/
│   ├── ProductService/
│   └── Ecommerce/       # Biblioteca compartilhada (Models, DTOs, Enums)
├── frontend/
├── nginx/
│   └── nginx.conf
├── docker-compose.yml
└── README.md
```

## 🔧 Testes de Comunicação

### CURL

- 📌 Registrar usuário

```bash
curl -X POST http://localhost:5000/api/auth/register -H "Content-Type: application/json" -d '{"name":"Alex","email":"alex@teste.com","password":"123456"}'
```

- 📌 Login

```bash
curl -X POST http://localhost:5000/api/auth/login -H "Content-Type: application/json" -d '{"email":"alex@teste.com","password":"123456"}'
```

- 📌 Listar produtos

```bash
curl -H "Authorization: Bearer SEU_TOKEN" http://localhost:5002/api/products
```

## 📝 Comandos Úteis

- Parar e remover containers

```bash
docker compose down
```

- Rebuild sem cache

```bash
docker compose build --no-cache
```

- Verificar containers ativos

```bash
docker ps
```

- Ver logs de um serviço

```bash
docker logs authservice
```

- Testar comunicação entre containers

```bash
docker exec -it frontend ping authservice
```

## 🔒 Observações Técnicas

- **Node.js:** Versão mínima recomendada: **v18+**

> Node 16 não possui suporte nativo ao módulo `crypto` em ESM puro, use a versao 20 ou superior.

- **CORS:** Configurado no `Program.cs` de cada serviço via:

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

app.UseCors("AllowAll");
```

- **NGINX:** Faz o roteamento interno entre serviços via upstreams configurados no `nginx/nginx.conf`.

## 🧠 Contribuição

1. Crie sua branch:

```bash
git checkout -b feature/nova-feature
```

2. Faça o commit:

```bash
git commit -m "feat: nova feature"
```

3. Faça push:

```bash
git push origin feature/nova-feature
```

4. Abra um Pull Request para `dev`.

## 🏆 Autor

Desenvolvido por **Alexandre de Souza Eustáquio**

## 📜 Licença

Este projeto está sob licença **MIT**.