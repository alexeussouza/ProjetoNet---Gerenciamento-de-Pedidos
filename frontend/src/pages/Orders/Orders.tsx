import { useEffect, useState } from 'react';
import { OrderService } from '../../api/ApiService';
import type { Order } from '../../types';
import { useAuth } from '../../contexts/AuthContext';
import './Orders.css';

export default function Orders() {
  // Estado para armazenar os pedidos
  const [orders, setOrders] = useState<Order[]>([]);
  // Estado para controle de carregamento
  const [loading, setLoading] = useState<boolean>(false);
  // Estado para armazenar mensagens de erro
  const [error, setError] = useState<string | null>(null);

  // Obtém o token do usuário autenticado
  const { token } = useAuth();

  // Carrega os pedidos quando o componente é montado
  useEffect(() => {
    if (token) {
      setLoading(true);        // Inicia o loader
      setError(null);          // Reseta erros anteriores
      OrderService.getOrders(token)
        .then(setOrders)       // Salva os pedidos no estado
        .catch(() => setError('Erro ao carregar pedidos.')) // Se der erro, salva a mensagem
        .finally(() => setLoading(false)); // Para o loader
    }
  }, [token]);

  return (
    <div>
      <h1>Meus Pedidos</h1>

      {/* Loader durante o carregamento */}
      {loading && <p>🔄 Carregando pedidos...</p>}

      {/* Mensagem de erro, se houver */}
      {error && <p style={{ color: 'red' }}>{error}</p>}

      {/* Lista de pedidos */}
      <ul>
        {orders.map((order) => (
          <li key={order.id}>
            Pedido #{order.id} - Produto: {order.productName} - Status: {order.status}
          </li>
        ))}
      </ul>

      {/* Se não houver pedidos e não estiver carregando */}
      {!loading && orders.length === 0 && <p>📭 Você ainda não fez pedidos.</p>}
    </div>
  );
}