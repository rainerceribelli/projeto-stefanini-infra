import api from '../api/axios';

class AuthService {
  async login({ username, password }) {
    try {
      const response = await api.post('Auth/login', { // <--- usa apenas o endpoint
        Id: 0,
        UserName: username,
        Password: password,
        Role: "string"
      });

      return response.data;
    } catch (error) {
      console.error('Erro ao fazer login:', error.response?.data || error.message);
      throw error;
    }
  }

  async logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    return true;
  }

  isAuthenticated() {
    return !!localStorage.getItem('token');
  }

  getToken() {
    return localStorage.getItem('token');
  }

  getUser() {
    const user = localStorage.getItem('user');
    return user ? JSON.parse(user) : null;
  }

  saveAuthData(user, token) {
    localStorage.setItem('user', JSON.stringify(user));
    localStorage.setItem('token', token);
  }

  setupAxiosInterceptors() {
    api.interceptors.request.use(
      (config) => {
        const token = this.getToken();
        if (token) {
          config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
      },
      (error) => Promise.reject(error)
    );
  }
}

const authService = new AuthService();
authService.setupAxiosInterceptors();

export default authService;
