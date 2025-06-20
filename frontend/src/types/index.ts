// 🔸 Tipagem global utilizada em todo o projeto

// 🔹 Usuário
export interface User {
  id: number;        // ID do usuário
  name: string;      // Nome do usuário
  email: string;     // Email do usuário
}

// 🔹 Produto
export interface Product {
  id: number;        // ID do produto
  name: string;      // Nome do produto
  price: number;     // Preço do produto
}

// 🔹 Pedido
export interface Order {
  id: number;            // ID do pedido
  productName: string;   // Nome do produto relacionado ao pedido
  status: string;        // Status do pedido (ex: "Pendente", "Concluído")
}

// 🔹 Resposta da autenticação
export interface AuthResponse {
  token: string;         // Token JWT retornado na autenticação
}