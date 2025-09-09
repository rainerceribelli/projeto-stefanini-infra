import React, { useState, useEffect } from "react";
import { Form, message, Modal } from "antd";
import moment from "moment";
import { useAuth } from "../../context/AuthContext";
import pessoaService from "../../services/pessoaService";
import PageHeader from "../../components/PageHeader";
import PessoaGrid from "./Components/Grid/PessoaGrid";
import PessoaModal from "../../components/PessoaModal";
import { CleanCpf, CleanTelefone } from "../../Utils/Functions";

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
      console.log(data);
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
      if (erros) {
        const fields = Object.keys(erros).map((key) => ({
          name: key,
          errors: erros[key],
        }));
        form.setFields(fields);
      } else {
        message.error("Erro ao criar pessoa!");
      }
    }
  };

  const handleEdit = (pessoa) => {
    setEditingPessoa(pessoa);
    form.setFieldsValue({
      nome: pessoa.nome,
      cpf: pessoa.cpf,
      dataNascimento: pessoa.dataNascimento
        ? moment(pessoa.dataNascimento)
        : null,
      email: pessoa.email,
      telefone: pessoa.telefone,
      naturalidade: pessoa.naturalidade,
      nacionalidade: pessoa.nacionalidade,
      sexo: pessoa.sexo,
      logradouro: pessoa.logradouro,
      numero: pessoa.numero,
      cidade: pessoa.cidade,
      estado: pessoa.estado,
      cep: pessoa.cep,
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
        sexo: values.sexo,
        bitAtivo: editingPessoa.bitAtivo !== undefined ? editingPessoa.bitAtivo : true,
        endereco: {
          id: editingPessoa.endereco?.id || 0,
          rua: values.logradouro,
          numero: values.numero,
          cidade: values.cidade,
          estado: values.estado,
          cep: values.cep,
        },
      };


      await pessoaService.updatePessoa(pessoaDTO);
      message.success("Pessoa atualizada com sucesso!");
      form.resetFields();
      setModalVisible(false);
      setEditingPessoa(null);
      loadPessoas();
    } catch (error) {
      console.error("Erro ao atualizar pessoa:", error);
      console.error("Detalhes do erro:", error.response?.data);
      message.error(
        "Erro ao atualizar pessoa: " +
        (error.response?.data?.message || error.message)
      );
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
          console.error("Erro ao excluir pessoa:", error);
          message.error(
            "Erro ao excluir pessoa: " +
            (error.response?.data?.message || error.message)
          );
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
