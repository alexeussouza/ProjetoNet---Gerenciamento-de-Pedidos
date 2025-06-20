import type { Product, Order, AuthResponse, User } from '../types';
import { API_BASE_URL } from '../config';

// Base comum para todos os serviços via NGINX proxy
const BASE_API_URL = API_BASE_URL.replace(/\/$/, ''); // remove barra final

const AUTH_SERVICE_URL = `${BASE_API_URL}/api/auth`;
const ORDER_SERVICE_URL = `${BASE_API_URL}/api/orders`;
const PRODUCT_SERVICE_URL = `${BASE_API_URL}/api/products`;

// Serviço de autenticação
export const AuthService = {
  async login(email: string, password: string): Promise<AuthResponse> {
    const response = await fetch(`${AUTH_SERVICE_URL}/login`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ email, password }),
    });

    if (!response.ok) {
      throw new Error('Falha no login');
    }

    return response.json();
  },

  async register(name: string, email: string, password: string): Promise<User> {
    const response = await fetch(`${AUTH_SERVICE_URL}/register`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ name, email, password }),
    });

    if (!response.ok) {
      throw new Error('Falha no cadastro');
    }

    return response.json();
  },
};

// Serviço de produtos
export const ProductService = {
  async getProducts(token: string): Promise<Product[]> {
    const response = await fetch(PRODUCT_SERVICE_URL, {
      headers: { Authorization: `Bearer ${token}` },
    });

    if (!response.ok) {
      throw new Error('Erro ao buscar produtos');
    }

    return response.json();
  },
};

// Serviço de pedidos
export const OrderService = {
  async getOrders(token: string): Promise<Order[]> {
    const response = await fetch(ORDER_SERVICE_URL, {
      headers: { Authorization: `Bearer ${token}` },
    });

    if (!response.ok) {
      throw new Error('Erro ao buscar pedidos');
    }

    return response.json();
  },

  async createOrder(token: string, productId: number): Promise<Order> {
    const response = await fetch(ORDER_SERVICE_URL, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${token}`,
      },
      body: JSON.stringify({
        items: [{ productId, quantity: 1 }],
      }),
    });

    if (!response.ok) {
      throw new Error('Erro ao criar pedido');
    }

    return response.json();
  },
};
