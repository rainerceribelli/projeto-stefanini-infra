import { Card, Col, Form, Input, Row } from "antd";
import { useEffect, useState } from "react";
import { FormatCep, ValidaCep } from "../../../../Utils/Functions";
import { showError, showInfo } from "../../../../Notification";
import FormItemStyled from "../../../../components/FormItem/FormItem.styled";
import FormItem from "../../../../components/FormItem";

function EnderecoForm({ form, editar, visualizar }) {
  const [disabled, setDisabled] = useState(true);
  const dados = form.getFieldsValue();

  useEffect(() => {
    if (dados.cep) {
      const isCep = ValidaCep(FormatCep(dados.cep));

      if (isCep && editar) {
        getAdress(dados.cep);
      }
    }
  }, [editar, form, dados.cep]);

  async function getAdress(cep, cepChange) {
    fetch(`https://viacep.com.br/ws/${cep}/json/`)
      .then((response) => response.json())
      .then((data) => {
        if (data.erro) {
          showError(`O CEP ${cep} não foi localizado na base dos Correios!`);
        } else {
          form.setFieldsValue({
            cidade: data.localidade,
            estado: data.uf,
          });

          if (data.logradouro !== "") {
            form.setFieldsValue({
              logradouro: data.logradouro,
            });
          }

          if (data.bairro !== "") {
            form.setFieldsValue({
              bairro: data.bairro,
            });
          }

          setDisabled(true);

          if (data.logradouro == "" || data.bairro == "") {
            setDisabled(false);

            if (cepChange) {
              showInfo(
                "O CEP informado é genérico, digite o Logradouro e o Bairro!"
              );

              form.setFieldsValue({
                bairro: "",
                logradouro: "",
              });
            }
          }
        }
      })
      .catch((error) => {
        showError(`Erro ao buscar CEP: ${cep}. Erro: ${error} `);
      });
  }

  const validateField = (value, name) => {
    if (value === null || value === "") {
      return Promise.reject(`Digite o ${name}!`);
    }
    return Promise.resolve();
  };

  async function handleCepChange(CEP) {
    form.setFieldsValue({
      cep: CEP ? FormatCep(CEP) : "",
      cidade: "",
      estado: "",
      bairro: "",
      logradouro: "",
      numero: "",
      complemento: "",
    });

    const isCep = ValidaCep(FormatCep(CEP));

    if (isCep) {
      await getAdress(FormatCep(CEP), true);
    }
  }

  const validateFieldCep = (value) => {
    if (!ValidaCep(FormatCep(value))) {
      return Promise.reject(`Digite um CEP válido!`);
    }
    return Promise.resolve();
  };

  return (
    <Card title="Endereço" size="small">
      <Row gutter={[20, 20]}>
        <Col xs={22} md={4}>
          <FormItemStyled
            label="CEP"
            name="cep"
            rules={[
              () => ({
                validator(_, value) {
                  return validateFieldCep(value);
                },
              }),
            ]}
          >
            <Input
              autoComplete="off"
              onChange={(e) => handleCepChange(e.target.value)}
              disabled={visualizar}
            />
          </FormItemStyled>
        </Col>

        <Col xs={22} md={17}>
          <FormItem
            label="Logradouro"
            name="logradouro"
            rules={[
              {
                max: 255,
                message: "O Endereço deve ter no máximo 255 caracteres!",
              },
              () => ({
                validator(_, value) {
                  return validateField(value, "Endereço");
                },
              }),
            ]}
          >
            <Input
              disabled={disabled || visualizar}
              maxLength={255}
              showCount
              autoComplete="off"
            />
          </FormItem>
        </Col>

        <Col xs={22} md={3}>
          <FormItem
            label="Número"
            name="numero"
            rules={[{ required: true, message: "Digite o Número!" }]}
          >
            <Input autoComplete="off" disabled={visualizar} />
          </FormItem>
        </Col>
      </Row>

      <Row gutter={[20, 20]}>
        <Col xs={22} md={24}>
          <FormItem
            label="Complemento"
            name="complemento"
            rules={[
              {
                max: 255,
                message: "O Complemento deve ter no máximo 255 caracteres!",
              },
            ]}
          >
            <Input
              placeholder={
                visualizar ? "" : "Digite um complemento, se houver..."
              }
              maxLength={255}
              showCount
              autoComplete="off"
              disabled={visualizar}
            />
          </FormItem>
        </Col>
      </Row>

      <Row gutter={[20, 20]}>
        <Col xs={22} md={11}>
          <FormItem
            label="Bairro"
            name="bairro"
            rules={[
              {
                max: 70,
                message: "O Bairro deve ter no máximo 70 caracteres!",
              },
              () => ({
                validator(_, value) {
                  return validateField(value, "Bairro");
                },
              }),
            ]}
          >
            <Input
              disabled={visualizar}
              maxLength={70}
              showCount
              autoComplete="off"
            />
          </FormItem>
        </Col>

        <Col xs={22} md={11}>
          <FormItem
            label="Cidade"
            name="cidade"
            rules={[
              {
                max: 100,
                message: "A Cidade deve ter no máximo 100 caracteres!",
              },
            ]}
          >
            <Input disabled maxLength={100} showCount />
          </FormItem>
        </Col>

        <Col xs={22} md={2}>
          <FormItem label="UF" name="estado">
            <Input disabled />
          </FormItem>
        </Col>
      </Row>
    </Card>
  );
}

export default EnderecoForm;
