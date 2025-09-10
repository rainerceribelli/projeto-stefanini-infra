import React, { useState, useEffect } from "react";
import { Form, message, Modal } from "antd";
import moment from "moment";
import { useAuth } from "../../context/AuthContext";
import pessoaService from "../../services/pessoaService";
import PageHeader from "../../components/PageHeader";
import PessoaGrid from "./Components/Grid/PessoaGrid";
import PessoaModal from "../../components/PessoaModal";
import { CleanCEP, CleanCpf, CleanTelefone, FormatCep, FormatCpf, FormatTelefone } from "../../Utils/Functions";

const CadastroPessoa = () => {
  const [pessoas, setPessoas] = useState([]);
  const [loading, setLoading] = useState(false);
  const [modalVisible, setModalVisible] = useState(false);
  const [visualizar, setVisualizar] = useState(false);
  const [editar, setEditar] = useState(false);
  const [editingPessoa, setEditingPessoa] = useState(null);
  const [form] = Form.useForm();
  const { logout, user } = useAuth();

  const loadPessoas = async () => {
    setLoading(true);
    try {
      const data = await pessoaService.getListPessoas();
      setPessoas(data || []);
    } catch (error) {
      console.error("Erro ao carregar pessoas:", error);
      message.error("Erro ao carregar lista de pessoas");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadPessoas();
  }, []);

  const handleCreate = async (values) => {
    try {
      const pessoaDTO = {
        nome: values.nome,
        cpf: CleanCpf(values.cpf),
        dataNascimento: values.dataNascimento,
        email: values.email,
        telefone: CleanTelefone(values.telefone),
        nacionalidade: values.nacionalidade,
        naturalidade: values.naturalidade,
        Endereco: {
          cep: values.cep,
          logradouro: values.logradouro,
          numero: values.numero,
          complemento: values.complemento,
          bairro: values.bairro,
          cidade: values.cidade,
          estado: values.estado,
        },
      };

      await pessoaService.createPessoa(pessoaDTO);
      message.success("Pessoa criada com sucesso!");
      form.resetFields();
      setModalVisible(false);
      loadPessoas();

    } catch (error) {
      const erros = error.response?.data?.erros;
      if (Array.isArray(erros)) {
        erros.forEach((msg) => message.error(msg));
      } else {
        message.error("Erro ao criar pessoa!");
      }
    }
  };

  const handleEdit = (pessoa) => {
    setEditingPessoa(pessoa);
    form.setFieldsValue({
      nome: pessoa.nome,
      cpf: FormatCpf(pessoa.cpf),
      dataNascimento: pessoa.dataNascimento
        ? moment(pessoa.dataNascimento, "YYYY-MM-DD")
        : null,
      email: pessoa.email,
      telefone: FormatTelefone(pessoa.telefone),
      naturalidade: pessoa.naturalidade,
      nacionalidade: pessoa.nacionalidade,
      sexo: pessoa.sexo,
      logradouro: pessoa.logradouro,
      numero: pessoa.numero,
      cidade: pessoa.cidade,
      estado: pessoa.estado,
      cep: FormatCep(pessoa.cep),
      bairro: pessoa.bairro,
      complemento: pessoa.complemento,
    });
    setModalVisible(true);
  };

  const handleUpdate = async (values) => {
    try {
      const pessoaDTO = {
        id: editingPessoa.id,
        tipo: editingPessoa.tipo || 0,
        nome: values.nome,
        cpf: CleanCpf(values.cpf),
        dataNascimento: values.dataNascimento,
        email: values.email,
        telefone: CleanTelefone(values.telefone),
        nacionalidade: values.nacionalidade,
        naturalidade: values.naturalidade,
        bitAtivo: editingPessoa.bitAtivo !== undefined ? editingPessoa.bitAtivo : true,
        endereco: {
          id: editingPessoa.endereco?.id || 0,
          rua: values.logradouro,
          numero: values.numero,
          complemento: values.complemento,
          bairro: values.bairro,
          cidade: values.cidade,
          estado: values.estado,
          cep: CleanCEP(values.cep),
        },
      };


      await pessoaService.updatePessoa(pessoaDTO);
      message.success("Pessoa atualizada com sucesso!");
      form.resetFields();
      setModalVisible(false);
      setEditingPessoa(null);
      loadPessoas();
    } catch (error) {
      const erros = error.response?.data?.erros;
      if (Array.isArray(erros)) {
        erros.forEach((msg) => message.error(msg));
      } else {
        message.error("Erro ao criar pessoa!");
      }
    }
  };

  const handleCancel = () => {
    setModalVisible(false);
    setEditingPessoa(null);
    form.resetFields();
  };

  const handleAddClick = () => {
    setEditingPessoa(null);
    setVisualizar(false);
    setEditar(false);
    form.resetFields();
    setModalVisible(true);
  };

  const handleDelete = (pessoa) => {
    Modal.confirm({
      title: "Confirmar Exclusão",
      content: `Tem certeza que deseja excluir a pessoa "${pessoa.nome}"?`,
      okText: "Sim, excluir",
      cancelText: "Cancelar",
      okType: "danger",
      onOk: async () => {
        try {
          await pessoaService.deletePessoa(pessoa.id);
          message.success("Pessoa excluída com sucesso!");
          loadPessoas();
        } catch (error) {
          const erros = error.response?.data?.erros;
          if (Array.isArray(erros)) {
            erros.forEach((msg) => message.error(msg));
          } else {
            message.error("Erro ao criar pessoa!");
          }
        }
      },
    });
  };

  return (
    <div style={{ padding: "24px", background: "#f0f2f5", minHeight: "100vh" }}>
      <PageHeader
        title="Cadastro de Pessoas"
        subtitle={`Bem-vindo, ${user?.username}! Gerencie o cadastro de pessoas.`}
        onAddClick={handleAddClick}
        onLogout={logout}
        user={user}
      />

      <PessoaGrid
        pessoas={pessoas}
        loading={loading}
        onEdit={handleEdit}
        onRefresh={loadPessoas}
        setOpen={setModalVisible}
        setVisualizar={setVisualizar}
        setEditar={setEditar}
        form={form}
        onDelete={handleDelete}
      />

      <PessoaModal
        visible={modalVisible}
        onCancel={handleCancel}
        onFinish={editingPessoa ? handleUpdate : handleCreate}
        form={form}
        editingPessoa={editingPessoa}
        loading={loading}
        visualizar={visualizar}
        editar={editar}
      />
    </div>
  );
};

export default CadastroPessoa;
