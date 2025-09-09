using GestaoDeCadastro.Domain.Entities.Cadastro;
using Xunit;

namespace GestaoDeCadastro.Tests.Domain.Entities
{
    public class tEnderecoTests
    {
        [Fact]
        public void Construtor_ComDadosValidos_DeveCriarEnderecoValido()
        {
            var logradouro = "Rua das Flores";
            var numero = "123";
            var cidade = "São Paulo";
            var estado = "SP";
            var cep = "01234567";
            var pessoaId = 1;
            var complemento = "Apto 45";
            var bairro = "Centro";

            var endereco = new tEndereco(logradouro, numero, cidade, estado, cep, pessoaId, complemento, bairro);

            Assert.Equal(logradouro, endereco.Logradouro);
            Assert.Equal(numero, endereco.Numero);
            Assert.Equal(cidade, endereco.Cidade);
            Assert.Equal(estado.ToUpper(), endereco.Estado);
            Assert.Equal(cep, endereco.CEP);
            Assert.Equal(pessoaId, endereco.PessoaId);
            Assert.Equal(complemento, endereco.Complemento);
            Assert.Equal(bairro, endereco.Bairro);
            Assert.True(endereco.IsValid);
        }

        [Fact]
        public void Construtor_ComLogradouroVazio_DeveSerInvalido()
        {
            var logradouro = "";
            var numero = "123";
            var cidade = "São Paulo";
            var estado = "SP";
            var cep = "01234567";
            var pessoaId = 1;

            var endereco = new tEndereco(logradouro, numero, cidade, estado, cep, pessoaId);

            Assert.False(endereco.IsValid);
            Assert.Contains(endereco.Notifications, n => n.Key == "Logradouro");
        }

        [Fact]
        public void Construtor_ComNumeroVazio_DeveSerInvalido()
        {
            var logradouro = "Rua das Flores";
            var numero = "";
            var cidade = "São Paulo";
            var estado = "SP";
            var cep = "01234567";
            var pessoaId = 1;

            var endereco = new tEndereco(logradouro, numero, cidade, estado, cep, pessoaId);

            Assert.False(endereco.IsValid);
            Assert.Contains(endereco.Notifications, n => n.Key == "Numero");
        }

        [Fact]
        public void Construtor_ComCidadeVazia_DeveSerInvalido()
        {
            var logradouro = "Rua das Flores";
            var numero = "123";
            var cidade = "";
            var estado = "SP";
            var cep = "01234567";
            var pessoaId = 1;

            var endereco = new tEndereco(logradouro, numero, cidade, estado, cep, pessoaId);

            Assert.False(endereco.IsValid);
            Assert.Contains(endereco.Notifications, n => n.Key == "Cidade");
        }

        [Fact]
        public void Construtor_ComEstadoInvalido_DeveSerInvalido()
        {
            var logradouro = "Rua das Flores";
            var numero = "123";
            var cidade = "São Paulo";
            var estado = "S";
            var cep = "01234567";
            var pessoaId = 1;

            var endereco = new tEndereco(logradouro, numero, cidade, estado, cep, pessoaId);

            Assert.False(endereco.IsValid);
            Assert.Contains(endereco.Notifications, n => n.Key == "Estado");
        }

        [Fact]
        public void Construtor_ComCEPInvalido_DeveSerInvalido()
        {
            var logradouro = "Rua das Flores";
            var numero = "123";
            var cidade = "São Paulo";
            var estado = "SP";
            var cep = "12345";
            var pessoaId = 1;

            var endereco = new tEndereco(logradouro, numero, cidade, estado, cep, pessoaId);

            Assert.False(endereco.IsValid);
            Assert.Contains(endereco.Notifications, n => n.Key == "CEP");
        }

        [Fact]
        public void Construtor_ComPessoaIdZero_DeveSerInvalido()
        {
            var logradouro = "Rua das Flores";
            var numero = "123";
            var cidade = "São Paulo";
            var estado = "SP";
            var cep = "01234567";
            var pessoaId = 0;

            var endereco = new tEndereco(logradouro, numero, cidade, estado, cep, pessoaId);

            Assert.False(endereco.IsValid);
            Assert.Contains(endereco.Notifications, n => n.Key == "PessoaId");
        }

        [Fact]
        public void AtualizarEndereco_ComDadosValidos_DeveAtualizar()
        {
            var endereco = new tEndereco("Rua das Flores", "123", "São Paulo", "SP", "01234567", 1);
            var novoLogradouro = "Avenida Paulista";
            var novoNumero = "456";
            var novaCidade = "São Paulo";
            var novoEstado = "SP";
            var novoCep = "01310100";
            var novoComplemento = "Sala 12";
            var novoBairro = "Bela Vista";

            endereco.AtualizarEndereco(novoLogradouro, novoNumero, novaCidade, novoEstado, novoCep, novoComplemento, novoBairro);

            Assert.Equal(novoLogradouro, endereco.Logradouro);
            Assert.Equal(novoNumero, endereco.Numero);
            Assert.Equal(novaCidade, endereco.Cidade);
            Assert.Equal(novoEstado.ToUpper(), endereco.Estado);
            Assert.Equal(novoCep, endereco.CEP);
            Assert.Equal(novoComplemento, endereco.Complemento);
            Assert.Equal(novoBairro, endereco.Bairro);
            Assert.True(endereco.IsValid);
        }

        [Fact]
        public void ObterEnderecoCompleto_DeveRetornarEnderecoFormatado()
        {
            var endereco = new tEndereco("Rua das Flores", "123", "São Paulo", "SP", "01234567", 1, "Apto 45", "Centro");

            var enderecoCompleto = endereco.ObterEnderecoCompleto();

            Assert.Equal("Rua das Flores, 123 (Apto 45) - Centro, São Paulo/SP - 01234-567", enderecoCompleto);
        }

        [Fact]
        public void ObterEnderecoCompleto_SemComplementoEBairro_DeveRetornarEnderecoFormatado()
        {
            var endereco = new tEndereco("Rua das Flores", "123", "São Paulo", "SP", "01234567", 1);

            var enderecoCompleto = endereco.ObterEnderecoCompleto();

            Assert.Equal("Rua das Flores, 123, São Paulo/SP - 01234-567", enderecoCompleto);
        }

        [Theory]
        [InlineData("01234567", true)]
        [InlineData("01234-567", true)]
        [InlineData("01234 567", true)]
        [InlineData("1234567", false)]
        [InlineData("012345678", false)]
        [InlineData("abc12345", false)]
        public void IsValidCEP_DeveValidarCorretamente(string cep, bool esperado)
        {
            var logradouro = "Rua das Flores";
            var numero = "123";
            var cidade = "São Paulo";
            var estado = "SP";
            var pessoaId = 1;

            var endereco = new tEndereco(logradouro, numero, cidade, estado, cep, pessoaId);

            Assert.Equal(esperado, endereco.IsValid);
        }
    }
}
