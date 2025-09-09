import React from 'react';
import { Card, Button, Typography, Space, Row, Col } from 'antd';
import { PlusOutlined, LogoutOutlined } from '@ant-design/icons';

const { Title } = Typography;

const PageHeader = ({ 
  title, 
  subtitle, 
  onAddClick, 
  onLogout, 
  user,
  extra 
}) => {
  return (
    <Card
      style={{
        borderRadius: '12px',
        boxShadow: '0 4px 12px rgba(0,0,0,0.1)',
        marginBottom: '24px'
      }}
    >
      <Row justify="space-between" align="middle">
        <Col>
          <Title level={2} style={{ margin: 0, color: '#1890ff' }}>
            {title}
          </Title>
          {subtitle && (
            <p style={{ margin: '8px 0 0 0', color: '#666' }}>
              {subtitle}
            </p>
          )}
        </Col>
        <Col>
          <Space>
            {extra}
            <Button
              type="primary"
              icon={<PlusOutlined />}
              onClick={onAddClick}
              size="large"
              style={{ borderRadius: '8px' }}
            >
              Nova Pessoa
            </Button>
            <Button
              icon={<LogoutOutlined />}
              onClick={onLogout}
              size="large"
              style={{ borderRadius: '8px' }}
            >
              Sair
            </Button>
          </Space>
        </Col>
      </Row>
    </Card>
  );
};

export default PageHeader;
