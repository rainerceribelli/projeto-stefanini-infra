import axios from 'axios';

let apiUrl = '';

if (window.location.hostname === 'localhost') {
  apiUrl = 'https://localhost:5000';
} else {
  apiUrl = `${window.location.protocol}//${window.location.hostname}:5000`;
}

const api = axios.create({
  baseURL: `${apiUrl}/api`,
  headers: { 'Content-Type': 'application/json' },
});

api.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('token');
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => Promise.reject(error)
);

api.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 401) {
      localStorage.removeItem('token');
      localStorage.removeItem('user');
      window.location.href = '/login';
    }
    return Promise.reject(error);
  }
);

export default api;
