using GestaoDeCadastro.Domain.Entities.Cadastro;
using Xunit;

namespace GestaoDeCadastro.Tests.Domain.Entities
{
    public class tPessoaTests
    {
        [Fact]
        public void Construtor_ComDadosValidos_DeveCriarPessoaValida()
        {
            var nome = "João Silva";
            var cpf = "11144477735";
            var dataNascimento = new DateTime(1990, 1, 1);
            var email = "joao@email.com";
            var naturalidade = "São Paulo";
            var nacionalidade = "Brasileira";

            var pessoa = new tPessoa(nome, cpf, dataNascimento, email, naturalidade, nacionalidade);

            Assert.Equal(nome, pessoa.Nome);
            Assert.Equal(cpf, pessoa.CPF);
            Assert.Equal(dataNascimento, pessoa.DataNascimento);
            Assert.Equal(email, pessoa.Email);
            Assert.Equal(naturalidade, pessoa.Naturalidade);
            Assert.Equal(nacionalidade, pessoa.Nacionalidade);
            Assert.True(pessoa.BitAtivo);
            Assert.True(pessoa.IsValid);
        }

        [Fact]
        public void Construtor_ComNomeVazio_DeveSerInvalido()
        {
            var nome = "";
            var cpf = "11144477735";
            var dataNascimento = new DateTime(1990, 1, 1);

            var pessoa = new tPessoa(nome, cpf, dataNascimento);

            Assert.False(pessoa.IsValid);
            Assert.Contains(pessoa.Notifications, n => n.Key == "Nome");
        }

        [Fact]
        public void Construtor_ComCPFInvalido_DeveSerInvalido()
        {
            var nome = "João Silva";
            var cpf = "12345678900";
            var dataNascimento = new DateTime(1990, 1, 1);

            var pessoa = new tPessoa(nome, cpf, dataNascimento);

            Assert.False(pessoa.IsValid);
            Assert.Contains(pessoa.Notifications, n => n.Key == "CPF");
        }

        [Fact]
        public void Construtor_ComDataNascimentoFutura_DeveSerInvalido()
        {
            var nome = "João Silva";
            var cpf = "11144477735";
            var dataNascimento = DateTime.Now.AddDays(1);

            var pessoa = new tPessoa(nome, cpf, dataNascimento);

            Assert.False(pessoa.IsValid);
            Assert.Contains(pessoa.Notifications, n => n.Key == "DataNascimento");
        }

        [Fact]
        public void Construtor_ComEmailInvalido_DeveSerInvalido()
        {
            var nome = "João Silva";
            var cpf = "11144477735";
            var dataNascimento = new DateTime(1990, 1, 1);
            var email = "email-invalido";

            var pessoa = new tPessoa(nome, cpf, dataNascimento, email);

            Assert.False(pessoa.IsValid);
            Assert.Contains(pessoa.Notifications, n => n.Key == "Email");
        }

        [Fact]
        public void AtualizarNome_ComNomeValido_DeveAtualizar()
        {
            var pessoa = new tPessoa("João Silva", "11144477735", new DateTime(1990, 1, 1));
            var novoNome = "João Santos";

            pessoa.AtualizarNome(novoNome);

            Assert.Equal(novoNome, pessoa.Nome);
            Assert.True(pessoa.IsValid);
        }

        [Fact]
        public void AtualizarNome_ComNomeVazio_DeveSerInvalido()
        {
            var pessoa = new tPessoa("João Silva", "11144477735", new DateTime(1990, 1, 1));
            var novoNome = "";

            pessoa.AtualizarNome(novoNome);

            Assert.False(pessoa.IsValid);
            Assert.Contains(pessoa.Notifications, n => n.Key == "Nome");
        }

        [Fact]
        public void AtualizarEmail_ComEmailValido_DeveAtualizar()
        {
            var pessoa = new tPessoa("João Silva", "11144477735", new DateTime(1990, 1, 1));
            var novoEmail = "joao.novo@email.com";

            pessoa.AtualizarEmail(novoEmail);

            Assert.Equal(novoEmail, pessoa.Email);
            Assert.True(pessoa.IsValid);
        }

        [Fact]
        public void AtualizarEmail_ComEmailInvalido_DeveSerInvalido()
        {
            var pessoa = new tPessoa("João Silva", "11144477735", new DateTime(1990, 1, 1));
            var novoEmail = "email-invalido";

            pessoa.AtualizarEmail(novoEmail);

            Assert.False(pessoa.IsValid);
            Assert.Contains(pessoa.Notifications, n => n.Key == "Email");
        }

        [Fact]
        public void AtualizarCPF_ComCPFValido_DeveAtualizar()
        {
            var pessoa = new tPessoa("João Silva", "11144477735", new DateTime(1990, 1, 1));
            var novoCPF = "12345678909";

            pessoa.AtualizarCPF(novoCPF);

            Assert.Equal(novoCPF, pessoa.CPF);
            Assert.True(pessoa.IsValid);
        }

        [Fact]
        public void AtualizarCPF_ComCPFInvalido_DeveSerInvalido()
        {
            var pessoa = new tPessoa("João Silva", "11144477735", new DateTime(1990, 1, 1));
            var novoCPF = "12345678900";

            pessoa.AtualizarCPF(novoCPF);

            Assert.False(pessoa.IsValid);
            Assert.Contains(pessoa.Notifications, n => n.Key == "CPF");
        }

        [Fact]
        public void Ativar_DeveDefinirBitAtivoComoTrue()
        {
            var pessoa = new tPessoa("João Silva", "11144477735", new DateTime(1990, 1, 1));
            pessoa.Desativar();

            pessoa.Ativar();

            Assert.True(pessoa.BitAtivo);
        }

        [Fact]
        public void Desativar_DeveDefinirBitAtivoComoFalse()
        {
            var pessoa = new tPessoa("João Silva", "11144477735", new DateTime(1990, 1, 1));

            pessoa.Desativar();

            Assert.False(pessoa.BitAtivo);
        }

        [Theory]
        [InlineData("11144477735", true)]
        [InlineData("12345678909", true)]
        [InlineData("11111111111", false)]
        [InlineData("12345678900", false)]
        [InlineData("123", false)]
        public void IsValidCPF_DeveValidarCorretamente(string cpf, bool esperado)
        {
            var pessoa = new tPessoa("João Silva", cpf, new DateTime(1990, 1, 1));

            Assert.Equal(esperado, pessoa.IsValid);
        }
    }
}
