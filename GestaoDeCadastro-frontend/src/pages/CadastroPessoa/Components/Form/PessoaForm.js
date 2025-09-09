import React, { useState, useEffect } from "react";
import { Form, Input, Button, Space, Row, Col, Tag, DatePicker, Select, Spin } from "antd";
import EnderecoForm from "./EnderecoForm";
import { FormatCpf, FormatTelefone } from "../../../../Utils/Functions";

function PessoaForm({
  form,
  onFinish,
  onCancel,
  editingPessoa,
  loading = false,
  visualizar = false,
  editar = false,
}) {
  const [paises, setPaises] = useState([]);
  const [loadingPaises, setLoadingPaises] = useState(false);

  const buscarPaises = async () => {
    if (paises.length > 0) return;
    
    setLoadingPaises(true);
    try {
      const response = await fetch('https://restcountries.com/v3.1/all?fields=name,translations');
      const data = await response.json();
      
      const paisesFormatados = data
        .map(pais => ({
          value: pais.translations.por?.common || pais.name.common,
          label: pais.translations.por?.common || pais.name.common
        }))
        .sort((a, b) => a.label.localeCompare(b.label, 'pt-BR'));
      
      setPaises(paisesFormatados);
    } catch (error) {
      console.error('Erro ao buscar países:', error);
      const paisesFallback = [
        "Brasil", "Argentina", "Bolívia", "Chile", "Colômbia", "Equador", "Guiana",
        "Paraguai", "Peru", "Suriname", "Uruguai", "Venezuela", "Estados Unidos",
        "Canadá", "México", "Portugal", "Espanha", "França", "Itália", "Alemanha"
      ].map(pais => ({ value: pais, label: pais }));
      setPaises(paisesFallback);
    } finally {
      setLoadingPaises(false);
    }
  };

  useEffect(() => {
    buscarPaises();
  }, []);
  return (
    <Form form={form} layout="vertical" onFinish={onFinish}>
      <Row gutter={16}>
        <Col span={12}>
          <Form.Item
            label="Nome"
            name="nome"
            rules={[{ required: true, message: "Nome é obrigatório!" }]}
          >
            <Input placeholder="Digite o nome" disabled={visualizar} />
          </Form.Item>
        </Col>
        <Col span={12}>
          <Form.Item
            label="CPF"
            name="cpf"
            rules={[
              { required: true, message: "CPF é obrigatório!" },
              { 
                pattern: /^\d{3}\.\d{3}\.\d{3}-\d{2}$/, 
                message: "CPF deve ter o formato: 000.000.000-00" 
              },
            ]}
          >
            <Input 
              placeholder="Digite o CPF (000.000.000-00)" 
              maxLength={14}
              disabled={visualizar}
              onChange={(e) => {
                const formatted = FormatCpf(e.target.value);
                form.setFieldValue('cpf', formatted);
              }}
            />
          </Form.Item>
        </Col>
      </Row>

      <Form.Item
        label="Data de Nascimento"
        name="dataNascimento"
        rules={[
          { required: true, message: "Data de nascimento é obrigatória!" },
        ]}
      >
        <DatePicker 
          format="DD/MM/YYYY" 
          disabled={visualizar}
          placeholder="Selecione a data de nascimento"
          style={{ width: '100%' }}
        />
      </Form.Item>

      <Row gutter={16}>
        <Col span={12}>
          <Form.Item
            label="E-mail"
            name="email"
            rules={[{ type: "email", message: "E-mail inválido!" }]}
          >
            <Input placeholder="Digite o e-mail" type="email" disabled={visualizar} />
          </Form.Item>
        </Col>
        <Col span={12}>
          <Form.Item label="Telefone" name="telefone">
            <Input 
              placeholder="Digite o telefone (00) 00000-0000" 
              maxLength={15}
              disabled={visualizar}
              onChange={(e) => {
                const formatted = FormatTelefone(e.target.value);
                form.setFieldValue('telefone', formatted);
              }}
            />
          </Form.Item>
        </Col>
      </Row>

      <Row gutter={16}>
        <Col span={12}>
          <Form.Item 
            label="Naturalidade" 
            name="naturalidade"
            rules={[
              { required: true, message: "Naturalidade é obrigatória!" },
              { min: 2, message: "Naturalidade deve ter pelo menos 2 caracteres!" }
            ]}
          >
            <Input 
              placeholder="Digite a naturalidade" 
              disabled={visualizar}
              maxLength={100}
            />
          </Form.Item>
        </Col>
        <Col span={12}>
          <Form.Item 
            label="Nacionalidade" 
            name="nacionalidade"
            rules={[
              { required: true, message: "Nacionalidade é obrigatória!" },
              { min: 2, message: "Nacionalidade deve ter pelo menos 2 caracteres!" }
            ]}
          >
            <Select
              placeholder={loadingPaises ? "Carregando países..." : "Selecione a nacionalidade"}
              disabled={visualizar || loadingPaises}
              showSearch
              allowClear
              loading={loadingPaises}
              filterOption={(input, option) =>
                option.label.toLowerCase().indexOf(input.toLowerCase()) >= 0
              }
              options={paises}
              notFoundContent={loadingPaises ? <Spin size="small" /> : "Nenhum país encontrado"}
            />
          </Form.Item>
        </Col>
      </Row>

      {!visualizar && (
        <Tag style={{ marginBottom: 5 }} color="blue">
          Digite o CEP para atualizar o endereço.
        </Tag>
      )}

      <EnderecoForm form={form} visualizar={visualizar} editar={editar} />

      <Form.Item style={{ marginBottom: 0, textAlign: "right", marginTop: "24px" }}>
        <Space size="middle">
          <Button onClick={onCancel} disabled={loading}>
            Cancelar
          </Button>
          {!visualizar && (
            <Button type="primary" htmlType="submit" loading={loading}>
              {editingPessoa ? "Atualizar" : "Criar"}
            </Button>
          )}
        </Space>
      </Form.Item>
    </Form>
  );
}

export default PessoaForm;
