import api from "../api/axios";

class PessoaService {
  // Buscar lista de pessoas
  async getListPessoas() {
    try {
      const response = await api.get("/v2/PessoaV2/GetListPessoas");
      return response.data;
    } catch (error) {
      console.error("Erro ao buscar lista de pessoas:", error);
      throw error;
    }
  }

  // Buscar pessoas por tipo
  async getListPessoasByTipo(tipo) {
    try {
      const response = await api.get(
        `/v2/PessoaV2/GetListPessoasByTipo?Tipo=${tipo}`
      );
      return response.data;
    } catch (error) {
      console.error("Erro ao buscar pessoas por tipo:", error);
      throw error;
    }
  }

  // Buscar pessoa por ID
  async getPessoaById(id) {
    try {
      const response = await api.get(`/v2/PessoaV2/GetPessoaById/${id}`);
      return response.data;
    } catch (error) {
      console.error("Erro ao buscar pessoa por ID:", error);
      throw error;
    }
  }

  // Criar nova pessoa
  async createPessoa(CreatePessoa) {
    try {
      const response = await api.post(
        "/v2/PessoaV2/CreatePessoa",
        CreatePessoa
      );
      return response.data;
    } catch (error) {
      console.error("Erro ao criar pessoa:", error);
      throw error;
    }
  }

  // Atualizar pessoa
  async updatePessoa(pessoaData) {
    try {
      const response = await api.put("/v2/PessoaV2/UpdatePessoa", pessoaData);
      return response.data;
    } catch (error) {
      console.error("Erro ao atualizar pessoa:", error);
      throw error;
    }
  }

  // Excluir pessoa
  async deletePessoa(id) {
    try {
      const response = await api.delete(`/v2/PessoaV2/DeletePessoa/${id}`);
      return response.data;
    } catch (error) {
      console.error("Erro ao excluir pessoa:", error);
      throw error;
    }
  }

  // Formatar dados para atualização
  formatUpdateData(values, editingPessoa) {
    return {
      id: editingPessoa.id,
      tipo: editingPessoa.tipo,
      nome: values.nome,
      cpf: values.cpf || editingPessoa.cpf,
      dataNascimento: values.dataNascimento
        ? new Date(values.dataNascimento).toISOString()
        : editingPessoa.dataNascimento,
      email: values.email || editingPessoa.email || null,
      telefone: values.telefone || editingPessoa.telefone || null,
      naturalidade: values.naturalidade || editingPessoa.naturalidade || null,
      nacionalidade:
        values.nacionalidade || editingPessoa.nacionalidade || null,
      sexo: values.sexo || editingPessoa.sexo || null,
      bitAtivo:
        editingPessoa.bitAtivo !== undefined ? editingPessoa.bitAtivo : true,
      logradouro: values.logradouro || editingPessoa.logradouro || "",
      numero: values.numero || editingPessoa.numero || "",
      cidade: values.cidade || editingPessoa.cidade || "",
      estado: values.estado || editingPessoa.estado || "",
      cep: values.cep || editingPessoa.cep || "",
      bairro: values.bairro || editingPessoa.bairro || "",
      complemento: values.complemento || editingPessoa.complemento || "",
    };
  }
}

const pessoaService = new PessoaService();
export default pessoaService;
