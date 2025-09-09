import React, { useState } from 'react';
import { Form, Input, Button, Card, message, Typography } from 'antd';
import { UserOutlined, LockOutlined } from '@ant-design/icons';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import authService from '../services/authService';

const { Title } = Typography;

const Login = () => {
  const [loading, setLoading] = useState(false);
  const { login } = useAuth();
  const navigate = useNavigate();

  const onFinish = async (values) => {
    setLoading(true);
    try {
      const response = await authService.login({
        username: values.username,
        password: values.password
      });

      if (response.token) {
        login(
          { username: response.user.userName },
          response.token
        );
        
        message.success('Login realizado com sucesso!');
        navigate('/cadastro');
      }
    } catch (error) {
      console.error('Erro no login:', error);
      message.error('Usuário ou senha inválidos');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div style={{
      display: 'flex',
      justifyContent: 'center',
      alignItems: 'center',
      minHeight: '100vh',
      background: 'linear-gradient(135deg, #667eea 0%, #764ba2 100%)',
      padding: '20px'
    }}>
      <Card
        style={{
          width: '100%',
          maxWidth: 400,
          borderRadius: '12px',
          boxShadow: '0 8px 32px rgba(0,0,0,0.1)'
        }}
      >
        <div style={{ textAlign: 'center', marginBottom: '30px' }}>
          <Title level={2} style={{ color: '#1890ff', marginBottom: '10px' }}>
            Gestão de Cadastro
          </Title>
          <p style={{ color: '#666', margin: 0 }}>
            Faça login para acessar o sistema
          </p>
        </div>

        <Form
          name="login"
          onFinish={onFinish}
          autoComplete="off"
          layout="vertical"
        >
          <Form.Item
            label="Usuário"
            name="username"
            rules={[
              {
                required: true,
                message: 'Por favor, insira seu usuário!',
              },
            ]}
          >
            <Input
              prefix={<UserOutlined />}
              placeholder="Digite seu usuário"
              size="large"
            />
          </Form.Item>

          <Form.Item
            label="Senha"
            name="password"
            rules={[
              {
                required: true,
                message: 'Por favor, insira sua senha!',
              },
            ]}
          >
            <Input.Password
              prefix={<LockOutlined />}
              placeholder="Digite sua senha"
              size="large"
            />
          </Form.Item>

          <Form.Item>
            <Button
              type="primary"
              htmlType="submit"
              loading={loading}
              size="large"
              style={{
                width: '100%',
                height: '45px',
                borderRadius: '8px',
                fontSize: '16px',
                fontWeight: '500'
              }}
            >
              Entrar
            </Button>
          </Form.Item>
        </Form>

        <div style={{ textAlign: 'center', marginTop: '20px', color: '#999' }}>
          <p style={{ margin: 0, fontSize: '14px' }}>
            Credenciais de teste:
          </p>
          <p style={{ margin: '5px 0 0 0', fontSize: '12px' }}>
            <strong>Usuário:</strong> admin | <strong>Senha:</strong> admin123
          </p>
        </div>
      </Card>
    </div>
  );
};

export default Login;
