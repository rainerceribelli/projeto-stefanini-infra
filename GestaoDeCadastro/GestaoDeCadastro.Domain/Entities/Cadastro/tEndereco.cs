using Flunt.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GestaoDeCadastro.Domain.Entities.Cadastro
{
    public class tEndereco : Entity
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Rua é obrigatória")]
        [MaxLength(100, ErrorMessage = "Rua deve ter no máximo 100 caracteres")]
        public string Logradouro { get; private set; } = string.Empty;

        [Required(ErrorMessage = "Número é obrigatório")]
        [MaxLength(10, ErrorMessage = "Número deve ter no máximo 10 caracteres")]
        public string Numero { get; private set; } = string.Empty;

        [Required(ErrorMessage = "Cidade é obrigatória")]
        [MaxLength(50, ErrorMessage = "Cidade deve ter no máximo 50 caracteres")]
        public string Cidade { get; private set; } = string.Empty;

        [Required(ErrorMessage = "Estado é obrigatório")]
        [MaxLength(2, ErrorMessage = "Estado deve ter exatamente 2 caracteres")]
        [MinLength(2, ErrorMessage = "Estado deve ter exatamente 2 caracteres")]
        public string Estado { get; private set; } = string.Empty;

        [Required(ErrorMessage = "CEP é obrigatório")]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "CEP deve ter exatamente 8 dígitos")]
        public string CEP { get; private set; } = string.Empty;

        public string? Complemento { get; private set; }

        public string? Bairro { get; private set; }

        public DateTime DataCadastro { get; private set; } = DateTime.UtcNow;

        public DateTime? DataAtualizacao { get; private set; }

        public int PessoaId { get; private set; }

        public virtual tPessoa Pessoa { get; private set; } = null!;

        private tEndereco() { }

        public tEndereco(string logradouro, string numero, string cidade, string estado, string cep, int pessoaId, string? complemento = null, string? bairro = null)
        {
            Logradouro = logradouro;
            Numero = numero;
            Cidade = cidade;
            Estado = estado.ToUpper();
            CEP = LimparCEP(cep);
            PessoaId = pessoaId;
            Complemento = complemento;
            Bairro = bairro;
            DataCadastro = DateTime.UtcNow;

            AddNotifications(new Contract<tEndereco>()
                .Requires()
                .IsNotNullOrEmpty(logradouro, "Logradouro", "Logradouro é obrigatório")
                .IsLowerThan(logradouro.Length, 101, "Logradouro", "Logradouro deve ter no máximo 100 caracteres")
                .IsNotNullOrEmpty(numero, "Numero", "Número é obrigatório")
                .IsLowerThan(numero.Length, 11, "Numero", "Número deve ter no máximo 10 caracteres")
                .IsNotNullOrEmpty(cidade, "Cidade", "Cidade é obrigatória")
                .IsLowerThan(cidade.Length, 51, "Cidade", "Cidade deve ter no máximo 50 caracteres")
                .IsNotNullOrEmpty(estado, "Estado", "Estado é obrigatório")
                .AreEquals(estado.Length, 2, "Estado", "Estado deve ter exatamente 2 caracteres")
                .IsNotNullOrEmpty(cep, "CEP", "CEP é obrigatório")
                .IsTrue(IsValidCEP(cep), "CEP", "CEP inválido")
                .IsGreaterThan(pessoaId, 0, "PessoaId", "ID da pessoa deve ser maior que zero")
            );
        }

        public void AtualizarEndereco(string novoLogradouro, string novoNumero, string novaCidade, string novoEstado, string novoCep, string? novoComplemento = null, string? novoBairro = null)
        {
            AddNotifications(new Contract<tEndereco>()
                .Requires()
                .IsNotNullOrEmpty(novoLogradouro, "Logradouro", "Logradouro é obrigatório")
                .IsLowerThan(novoLogradouro.Length, 101, "Logradouro", "Logradouro deve ter no máximo 100 caracteres")
                .IsNotNullOrEmpty(novoNumero, "Numero", "Número é obrigatório")
                .IsLowerThan(novoNumero.Length, 11, "Numero", "Número deve ter no máximo 10 caracteres")
                .IsNotNullOrEmpty(novaCidade, "Cidade", "Cidade é obrigatória")
                .IsLowerThan(novaCidade.Length, 51, "Cidade", "Cidade deve ter no máximo 50 caracteres")
                .IsNotNullOrEmpty(novoEstado, "Estado", "Estado é obrigatório")
                .AreEquals(novoEstado.Length, 2, "Estado", "Estado deve ter exatamente 2 caracteres")
                .IsNotNullOrEmpty(novoCep, "CEP", "CEP é obrigatório")
                .IsTrue(IsValidCEP(novoCep), "CEP", "CEP inválido")
            );

            if (IsValid)
            {
                Logradouro = novoLogradouro;
                Numero = novoNumero;
                Cidade = novaCidade;
                Estado = novoEstado.ToUpper();
                CEP = LimparCEP(novoCep);
                Complemento = novoComplemento;
                Bairro = novoBairro;
                SetLastUpdateDate();
            }
        }

        public string ObterEnderecoCompleto()
        {
            var complementoStr = string.IsNullOrWhiteSpace(Complemento) ? "" : $" ({Complemento})";
            var bairroStr = string.IsNullOrWhiteSpace(Bairro) ? "" : $" - {Bairro}";
            return $"{Logradouro}, {Numero}{complementoStr}{bairroStr}, {Cidade}/{Estado} - {FormatarCEP(CEP)}";
        }

        private static bool IsValidCEP(string cep)
        {
            cep = LimparCEP(cep);
            return cep.Length == 8 && cep.All(char.IsDigit);
        }

        private static string LimparCEP(string cep)
        {
            return Regex.Replace(cep, @"[^\d]", "");
        }

        private static string FormatarCEP(string cep)
        {
            cep = LimparCEP(cep);
            if (cep.Length == 8)
                return $"{cep.Substring(0, 5)}-{cep.Substring(5)}";
            return cep;
        }
    }
}
