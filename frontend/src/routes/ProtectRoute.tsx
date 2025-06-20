import type { ReactNode } from 'react';
import { Navigate, useLocation } from 'react-router-dom';
import { useAuth } from '../contexts/AuthContext';

/**
 * Tipagem para o componente de rota protegida.
 */
type Props = {
  children: ReactNode;
};

/**
 * Componente que protege rotas, redirecionando para /login se o usuário não estiver autenticado.
 * Se autenticado, renderiza o conteúdo normalmente.
 */
export function ProtectRoute({ children }: Props) {
  const { isAuthenticated } = useAuth();
  const location = useLocation();

  // Se não autenticado, redireciona para /login e salva a rota atual para possível redirecionamento pós-login
  if (!isAuthenticated) {
    return <Navigate to="/login" state={{ from: location }} replace />;
  }

  // Se autenticado, renderiza normalmente o conteúdo protegido
  return <>{children}</>;
}
