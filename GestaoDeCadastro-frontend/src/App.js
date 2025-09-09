import React from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import { ConfigProvider } from 'antd';
import ptBR from 'antd/locale/pt_BR';
import { AuthProvider } from './context/AuthContext';
import Login from './pages/Login';
import CadastroPessoa from './pages/CadastroPessoa';
import ProtectedRoute from './components/ProtectedRoute';
import 'antd/dist/reset.css';

function App() {
  return (
    <ConfigProvider locale={ptBR}>
      <AuthProvider>
        <Router>
          <div className="App">
            <Routes>
              <Route path="/login" element={<Login />} />
              <Route
                path="/cadastro"
                element={
                  <ProtectedRoute>
                    <CadastroPessoa />
                  </ProtectedRoute>
                }
              />
              <Route path="/" element={<Navigate to="/login" replace />} />
            </Routes>
          </div>
        </Router>
      </AuthProvider>
    </ConfigProvider>
  );
}

export default App;