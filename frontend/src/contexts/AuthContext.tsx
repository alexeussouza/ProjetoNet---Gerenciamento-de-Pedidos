import { createContext, useContext, useState, useEffect } from 'react';
import type { User } from '../types';
import type { ReactNode } from 'react';
import { AuthService } from '../api/ApiService';

interface AuthContextType {
  user: User | null;
  token: string | null;
  isAuthenticated: boolean;
  login: (email: string, password: string) => Promise<boolean>;
  register: (name: string, email: string, password: string) => Promise<boolean>;
  logout: () => void;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export function AuthProvider({ children }: { children: ReactNode }) {
  const [user, setUser] = useState<User | null>(null);
  const [token, setToken] = useState<string | null>(null);

  // Ao montar, tenta carregar o token do localStorage
  useEffect(() => {
    const storedToken = localStorage.getItem('token');
    if (storedToken) {
      setToken(storedToken);
      // Opcional: aqui você poderia buscar dados do usuário com o token, ou manter o usuário "anônimo"
    }
  }, []);

  const isAuthenticated = !!token;

  async function login(email: string, password: string): Promise<boolean> {
    try {
      const response = await AuthService.login(email, password);
      if (response.token) {
        localStorage.setItem('token', response.token);
        setToken(response.token);
        setUser({ email } as User); // ajuste conforme os dados reais
        return true;
      }
      return false;
    } catch (error) {
      console.error('Erro no login:', error);
      return false;
    }
  }

  async function register(name: string, email: string, password: string): Promise<boolean> {
    try {
      const newUser = await AuthService.register(name, email, password);
      if (newUser) {
        setUser(newUser);
        return true;
      }
      return false;
    } catch (error) {
      console.error('Erro no cadastro:', error);
      return false;
    }
  }

  function logout() {
    setUser(null);
    setToken(null);
    localStorage.removeItem('token');
  }

  return (
    <AuthContext.Provider value={{ user, token, isAuthenticated, login, register, logout }}>
      {children}
    </AuthContext.Provider>
  );
}

export function useAuth() {
  const context = useContext(AuthContext);
  if (!context) throw new Error('useAuth deve ser usado dentro do AuthProvider');
  return context;
}
