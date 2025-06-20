import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from '../contexts/AuthContext';


export default function Header() {
  const { isAuthenticated, logout } = useAuth();
  const navigate = useNavigate();
  
// Header visível apenas quando o usuário está logado
  if (!isAuthenticated) return null;

  const handleLogout = () => {
    logout();
    navigate('/login');
  };

  return (
    <header style={{ padding: '10px', background: '#f5f5f5', marginBottom: '20px' }}>
      <nav>
        <Link to="/products" style={{ marginRight: '10px' }}>Produtos</Link>
        <Link to="/orders" style={{ marginRight: '10px' }}>Meus Pedidos</Link>
        <button onClick={handleLogout}>Logout</button>
      </nav>
    </header>
  );
}
