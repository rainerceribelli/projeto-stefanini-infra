import React from "react";
import { Modal } from "antd";
import PessoaForm from "../pages/CadastroPessoa/Components/Form/PessoaForm";

function PessoaModal({
  visible,
  onCancel,
  onFinish,
  form,
  editingPessoa,
  loading = false,
  visualizar = false,
  editar = false,
}) {
  return (
    <Modal
      title={
        visualizar
          ? "Visualizar Pessoa"
          : editingPessoa
          ? "Editar Pessoa"
          : "Nova Pessoa"
      }
      open={visible}
      onCancel={onCancel}
      footer={null}
      width={800}
      style={{ borderRadius: "12px" }}
      destroyOnClose
    >
      <PessoaForm
        form={form}
        onFinish={onFinish}
        onCancel={onCancel}
        editingPessoa={editingPessoa}
        loading={loading}
        visualizar={visualizar}
        editar={editar}
      />
    </Modal>
  );
}

export default PessoaModal;
