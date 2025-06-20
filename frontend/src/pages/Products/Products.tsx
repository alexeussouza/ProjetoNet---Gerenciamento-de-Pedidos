import { useEffect, useState } from 'react';
import { ProductService, OrderService } from '../../api/ApiService';
import type { Product } from '../../types';
import { useAuth } from '../../contexts/AuthContext';
import './Products.css';

export default function Products() {
  // Estado para armazenar a lista de produtos
  const [products, setProducts] = useState<Product[]>([]);
  // Estado para controle de carregamento
  const [loading, setLoading] = useState<boolean>(false);
  // Estado para armazenar mensagens de erro
  const [error, setError] = useState<string | null>(null);

  // Obtém o token do usuário autenticado
  const { token } = useAuth();

  // Carrega os produtos quando o componente é montado
  useEffect(() => {
    if (token) {
      setLoading(true);        // Inicia o loader
      setError(null);          // Reseta erros anteriores
      ProductService.getProducts(token)
        .then(setProducts)     // Salva os produtos no estado
        .catch(() => setError('Erro ao carregar produtos.')) // Se der erro, salva a mensagem
        .finally(() => setLoading(false)); // Para o loader
    }
  }, [token]);

  // Função para criar um pedido do produto selecionado
  const handleOrder = (productId: number) => {
    if (token) {
      OrderService.createOrder(token, productId)
        .then(() => alert('✅ Pedido realizado com sucesso!'))
        .catch(() => alert('❌ Erro ao realizar o pedido.'));
    }
  };

  return (
    <div>
      <h1>Produtos</h1>

      {/* Loader durante o carregamento */}
      {loading && <p>🔄 Carregando produtos...</p>}

      {/* Mensagem de erro, se houver */}
      {error && <p style={{ color: 'red' }}>{error}</p>}

      {/* Lista de produtos */}
      <ul>
        {products.map((product) => (
          <li key={product.id}>
            {product.name} - R$ {product.price.toFixed(2)}
            <button onClick={() => handleOrder(product.id)}>Fazer Pedido</button>
          </li>
        ))}
      </ul>

      {/* Se não houver produtos e não estiver carregando */}
      {!loading && products.length === 0 && <p>🛍️ Nenhum produto disponível.</p>}
    </div>
  );
}
