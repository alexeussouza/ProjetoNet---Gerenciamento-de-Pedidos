import { BrowserRouter, Routes, Route, Navigate, useLocation } from 'react-router-dom';
import Orders from './pages/Orders/Orders';
import Products from './pages/Products/Products';
import { Login } from './pages/Login/Login';
import { Register } from './pages/Register/Register';
import { ProtectRoute } from './routes/ProtectRoute';
import { AuthProvider } from './contexts/AuthContext';
import Header from './components/Header';

/**
 * Componente responsável por definir as rotas e a visibilidade do Header
 * O useLocation é usado para saber qual página está ativa e esconder o Header em /login e /register
 */
function AppRoutes() {
    const location = useLocation();

    // Define se o Header deve ser ocultado com base na rota atual
    const hideHeader = ["/login", "/register"].includes(location.pathname);

    return (
        <>
            {/* Mostra o Header se não estiver nas páginas de login ou registro */}
            {!hideHeader && <Header />}

            <Routes>
                {/* Redireciona a rota raiz "/" para a página de login */}
                <Route path="/" element={<Navigate to="/login" />} />

                {/* Rotas públicas — acessíveis sem autenticação */}
                <Route path="/login" element={<Login />} />
                <Route path="/register" element={<Register />} />

                {/* Rotas protegidas — exigem autenticação */}
                <Route
                    path="/products"
                    element={
                        <ProtectRoute>
                            <Products />
                        </ProtectRoute>
                    }
                />
                <Route
                    path="/orders"
                    element={
                        <ProtectRoute>
                            <Orders />
                        </ProtectRoute>
                    }
                />
            </Routes>
        </>
    );
}

/**
 * Componente principal da aplicação
 * Envolve todas as rotas dentro do BrowserRouter e AuthProvider
 */
function App() {
  return (
    <BrowserRouter>
      <AuthProvider>
        <AppRoutes />
      </AuthProvider>
    </BrowserRouter>
  );
}

export default App;
