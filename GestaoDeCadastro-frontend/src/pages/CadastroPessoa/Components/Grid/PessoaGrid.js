import { useRef } from 'react';
import DataGrid, {
  Column,
  FilterRow,
  Paging,
  HeaderFilter,
  Pager,
  Toolbar,
  Item
} from "devextreme-react/data-grid";
import "devextreme/dist/css/dx.light.css";
import { EditOutlined, EyeOutlined, DeleteOutlined } from "@ant-design/icons";
import { Card } from "antd";
import { Button as GridButton } from 'devextreme-react/button';
import GridAction from "../../../../components/GridAction";
import moment from "moment";
import ExcelJS from 'exceljs';

function PessoaGrid({ pessoas, setOpen, setVisualizar, setEditar, form, onDelete, onEdit }) {
  const gridRef = useRef(null);

  const LoadInfo = (data) => {
    form.setFieldsValue({
      nome: data.nome,
      cpf: data.cpf,
      dataNascimento: data.dataNascimento ? moment(data.dataNascimento, "YYYY-MM-DD") : null,
      email: data.email,
      telefone: data.telefone,
      nacionalidade: data.nacionalidade,
      naturalidade: data.naturalidade,
      cep: data.cep,
      logradouro: data.logradouro,
      numero: data.numero,
      complemento: data.complemento,
      bairro: data.bairro,
      cidade: data.cidade,
      estado: data.estado,
    });
    setOpen(true);
  };

  const actions = [
    {
      title: "Editar Pessoa",
      icon: <EditOutlined />,
      onClick: ({ data }) => {
        onEdit(data);
        setVisualizar(false);
        setEditar(true);
      },
    },
    {
      title: "Visualizar Pessoa",
      icon: <EyeOutlined />,
      onClick: ({ data }) => {
        LoadInfo(data);
        setVisualizar(true);
        setEditar(false);
      },
    },
    {
      title: "Excluir Pessoa",
      icon: <DeleteOutlined />,
      onClick: ({ data }) => {
        onDelete(data);
      },
    },
  ];

  const formatGridCpfCnpj = (value) => {
    if (!value) return "";
    const cleanValue = String(value).replace(/\D/g, "");
    if (cleanValue.length === 11) {
      return cleanValue.replace(/(\d{3})(\d{3})(\d{3})(\d{2})/, "$1.$2.$3-$4");
    }
    return cleanValue;
  };

  const formatGridTelefone = (value) => {
    if (!value) return "";
    const cleanValue = String(value).replace(/\D/g, "");
    if (cleanValue.length <= 10) {
      return cleanValue.replace(/(\d{2})(\d)/, "($1) $2").replace(/(\d{4})(\d)/, "$1-$2");
    } else if (cleanValue.length === 11) {
      return cleanValue.replace(/(\d{2})(\d)/, "($1) $2").replace(/(\d{5})(\d)/, "$1-$2");
    }
    return cleanValue;
  };

  return (
    <Card>
      <DataGrid
        id="gridPessoas"
        dataSource={pessoas}
        ref={gridRef}
        showColumnLines
        showRowLines
        showBorders
        columnHidingEnabled
        allowColumnResizing
        columnAutoWidth
        hoverStateEnabled
        errorRowEnabled
        columnResizingMode="nextColumn"
        allowColumnReordering
        export={{
          enabled: true,
          fileName: "Pessoas",
          allowExportSelectedData: false
        }}
        onExporting={(e) => {
          const workbook = new ExcelJS.Workbook();
          const worksheet = workbook.addWorksheet('Pessoas');
          
          const headers = ['ID', 'Nome', 'CPF/CNPJ', 'Data Nascimento', 'E-mail', 'Telefone', 'Endereço Completo'];
          worksheet.addRow(headers);
          
          const headerRow = worksheet.getRow(1);
          headerRow.font = { bold: true };
          headerRow.fill = {
            type: 'pattern',
            pattern: 'solid',
            fgColor: { argb: 'FFE0E0E0' }
          };
          
          pessoas.forEach(pessoa => {
            worksheet.addRow([
              pessoa.id,
              pessoa.nome,
              formatGridCpfCnpj(pessoa.cpf),
              pessoa.dataNascimento ? moment(pessoa.dataNascimento).format('DD/MM/YYYY') : '',
              pessoa.email,
              formatGridTelefone(pessoa.telefone),
              pessoa.enderecoCompleto
            ]);
          });
          
          worksheet.columns.forEach(column => {
            column.width = 15;
          });
          
          workbook.xlsx.writeBuffer().then((buffer) => {
            const blob = new Blob([buffer], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
            const url = window.URL.createObjectURL(blob);
            const link = document.createElement('a');
            link.href = url;
            link.download = 'Pessoas.xlsx';
            link.click();
            window.URL.revokeObjectURL(url);
          });
          
          e.cancel = true;
        }}
      >
        <FilterRow visible={true} />
        <HeaderFilter visible={true} allowSearch={true} />
        <Paging defaultPageSize={10} />
        <Pager
          showPageSizeSelector={true}
          allowedPageSizes={[10, 25, 50, 100]}
          showInfo={true}
        />
        <Column dataField="id" caption="ID" width={80} />
        <Column dataField="nome" caption="Nome" width={180} />
        <Column dataField="cpf" caption="CPF/CNPJ" format={formatGridCpfCnpj} />
        <Column dataField="dataNascimento" caption="Data Nascimento" dataType="date" />
        <Column dataField="email" caption="E-mail" />
        <Column dataField="telefone" caption="Telefone" format={formatGridTelefone} />
        <Column dataField="enderecoCompleto" caption="Endereço Completo" />
        <Column
          width={150}
          caption="Ação"
          alignment="center"
          fixed
          fixedPosition="right"
          cellRender={(Verificacoes) => (
            <GridAction item={Verificacoes} actions={actions} />
          )}
        />

        <Toolbar>
          <Item>
            <GridButton
              icon="refresh"
              onClick={() => {
                if (gridRef.current) {
                  const instance = gridRef.current.instance();
                  instance.clearFilter();
                  instance.clearSorting();
                }
              }}
              hint="Limpar filtros"
            />
          </Item>
          <Item name="exportButton" />
        </Toolbar>
      </DataGrid>
    </Card>
  );
}

export default PessoaGrid;
