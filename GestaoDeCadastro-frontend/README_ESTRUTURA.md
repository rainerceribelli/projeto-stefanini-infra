# Estrutura do Frontend - Gestão de Cadastro

## 📁 Estrutura de Pastas

```
frontend/src/
├── api/
│   └── axios.js                 # Configuração do Axios
├── components/
│   ├── PageHeader.js           # Cabeçalho da página
│   ├── PessoaForm.js           # Formulário de pessoa
│   ├── PessoaGrid.js           # Grid/tabela de pessoas
│   ├── PessoaModal.js          # Modal de pessoa
│   └── ProtectedRoute.js       # Rota protegida
├── context/
│   └── AuthContext.js          # Contexto de autenticação
├── pages/
│   ├── Login.js                # Página de login
│   └── CadastroPessoa.js       # Página principal de cadastro
├── services/
│   ├── authService.js          # Service de autenticação
│   └── pessoaService.js        # Service de pessoas
├── App.js                      # Componente principal
├── index.js                    # Ponto de entrada
└── setupProxy.js               # Configuração de proxy
```

## 🧩 Componentes

### PageHeader
- **Função**: Cabeçalho reutilizável das páginas
- **Props**: title, subtitle, onAddClick, onLogout, user, extra
- **Uso**: Exibe título, subtítulo e botões de ação

### PessoaGrid
- **Função**: Tabela de pessoas com ações
- **Props**: pessoas, loading, onEdit, onRefresh, onDelete
- **Uso**: Lista pessoas com paginação e ações de editar/excluir

### PessoaForm
- **Função**: Formulário de pessoa
- **Props**: form, onFinish, onCancel, editingPessoa, loading
- **Uso**: Formulário para criar/editar pessoas

### PessoaModal
- **Função**: Modal que contém o formulário
- **Props**: visible, onCancel, onFinish, form, editingPessoa, loading
- **Uso**: Modal para criar/editar pessoas

## 🔧 Services

### authService
- **Função**: Gerenciar autenticação
- **Métodos**:
  - `login(credentials)` - Fazer login
  - `logout()` - Fazer logout
  - `isAuthenticated()` - Verificar se está autenticado
  - `getToken()` - Obter token
  - `getUser()` - Obter usuário
  - `saveAuthData(user, token)` - Salvar dados de auth

### pessoaService
- **Função**: Gerenciar operações de pessoas
- **Métodos**:
  - `getListPessoas()` - Buscar lista de pessoas
  - `getListPessoasByTipo(tipo)` - Buscar por tipo
  - `getPessoaById(id)` - Buscar por ID
  - `createPessoa(pessoaData)` - Criar pessoa
  - `updatePessoa(pessoaData)` - Atualizar pessoa
  - `deletePessoa(id)` - Excluir pessoa
  - `formatCreateData(values)` - Formatar dados para criação
  - `formatUpdateData(values, editingPessoa)` - Formatar dados para atualização

## 🎯 Benefícios da Nova Estrutura

### ✅ **Separação de Responsabilidades**
- Services cuidam da lógica de API
- Componentes cuidam da apresentação
- Páginas orquestram os componentes

### ✅ **Reutilização**
- Componentes podem ser reutilizados
- Services centralizam lógica de negócio
- Formatação de dados padronizada

### ✅ **Manutenibilidade**
- Código mais organizado
- Fácil de encontrar e modificar
- Testes mais simples

### ✅ **Escalabilidade**
- Fácil adicionar novos componentes
- Services podem ser expandidos
- Estrutura preparada para crescimento

## 🚀 Como Usar

### Adicionar Novo Componente
1. Criar arquivo em `components/`
2. Exportar como default
3. Importar onde necessário

### Adicionar Novo Service
1. Criar arquivo em `services/`
2. Implementar métodos necessários
3. Exportar instância da classe

### Adicionar Nova Página
1. Criar arquivo em `pages/`
2. Usar componentes existentes
3. Adicionar rota em `App.js`

## 🔄 Fluxo de Dados

```
Página → Service → API → Backend
  ↓
Componente ← Service ← API ← Backend
```

1. **Página** chama **Service**
2. **Service** faz requisição para **API**
3. **API** comunica com **Backend**
4. **Dados** retornam para **Componente**
5. **Componente** atualiza a **Interface**
