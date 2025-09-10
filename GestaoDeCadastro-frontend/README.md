# Frontend - Gestão de Cadastro

Frontend desenvolvido em React com Ant Design para o sistema de gestão de cadastro de pessoas.

## 📋 Funcionalidades

### 🔐 Autenticação
- Tela de login com validação
- Gerenciamento de token JWT
- Redirecionamento automático após login
- Proteção de rotas

### 👥 Cadastro de Pessoas
- Listagem de pessoas em tabela
- Criação de novas pessoas
- Edição de pessoas existentes
- Exclusão com confirmação
- Formulário com validação

### 🎨 Interface
- Design responsivo e moderno
- Componentes do Ant Design
- Notificações de sucesso/erro
- Loading states

## 🛠️ Instalação e Execução

### Pré-requisitos
- Node.js (versão 16 ou superior)
- Backend da API rodando na porta 5041

### Instalação
```bash
# Instalar dependências
npm install

# Executar em modo de desenvolvimento
npm start
```

### Acesso
- **URL:** http://localhost:3000
- **Usuário:** admin
- **Senha:** admin123

## 📁 Estrutura do Projeto

```
src/
├── api/
│   └── axios.js          # Configuração do Axios
├── components/
│   └── ProtectedRoute.js # Componente de rota protegida
├── context/
│   └── AuthContext.js    # Context de autenticação
├── pages/
│   ├── Login.js          # Página de login
│   └── CadastroPessoa.js # Página de cadastro
├── App.js                # Componente principal
├── index.js              # Ponto de entrada
└── setupProxy.js         # Configuração de proxy
```

## 🔗 Integração com Backend

O frontend consome os seguintes endpoints da API:

### Autenticação
- `POST /api/Auth/Login` - Login do usuário

### Pessoas (Versão 2)
- `GET /api/v2/PessoaV2/GetListPessoas` - Listar pessoas
- `POST /api/v2/PessoaV2/CreatePessoa` - Criar pessoa
- `PUT /api/v2/PessoaV2/UpdatePessoa` - Atualizar pessoa
- `DELETE /api/v2/PessoaV2/DeletePessoa/{id}` - Excluir pessoa

## 🎯 Características

- **Responsivo:** Adapta-se a diferentes tamanhos de tela
- **Seguro:** Token JWT para autenticação
- **Intuitivo:** Interface amigável e fácil de usar
- **Robusto:** Tratamento de erros e validações
- **Moderno:** Utiliza as melhores práticas do React

## 🚀 Deploy

Para fazer o build de produção:

```bash
npm run build
```

Os arquivos estáticos serão gerados na pasta `build/`.