using Flunt.Validations;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace GestaoDeCadastro.Domain.Entities.Cadastro
{
    public class tPessoa : Entity
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Informar o nome da pessoa!")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres")]
        public string Nome { get; private set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Email inválido")]
        [StringLength(100, ErrorMessage = "O email deve ter no máximo 100 caracteres")]
        public string? Email { get; private set; }

        [Required(ErrorMessage = "Data de nascimento é obrigatória")]
        public DateTime DataNascimento { get; private set; }

        [StringLength(50, ErrorMessage = "A naturalidade deve ter no máximo 50 caracteres")]
        public string? Naturalidade { get; private set; }

        [StringLength(50, ErrorMessage = "A nacionalidade deve ter no máximo 50 caracteres")]
        public string? Nacionalidade { get; private set; }

        [Required(ErrorMessage = "CPF é obrigatório")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "CPF deve ter exatamente 11 dígitos")]
        public string CPF { get; private set; } = string.Empty;

        public string? Telefone { get; private set; }

        public virtual tEndereco? Endereco { get; private set; }

        public bool BitAtivo { get; private set; } = true;

        public DateTime DataCadastro { get; private set; } = DateTime.Now;

        public DateTime? DataAtualizacao { get; private set; }

        private tPessoa() { }

        public tPessoa(string nome, string cpf, DateTime dataNascimento, string? email = null, string? naturalidade = null, string? nacionalidade = null, string? telefone = null)
        {
            Nome = nome;
            CPF = cpf;
            DataNascimento = dataNascimento;
            Email = email;
            Naturalidade = naturalidade;
            Nacionalidade = nacionalidade;
            Telefone = telefone;
            BitAtivo = true;
            DataCadastro = DateTime.UtcNow;

            AddNotifications(new Contract<tPessoa>()
                .Requires()
                .IsNotNullOrEmpty(nome, "Nome", "Nome é obrigatório")
                .IsGreaterThan(nome.Length, 1, "Nome", "Nome deve ter pelo menos 2 caracteres")
                .IsLowerThan(nome.Length, 101, "Nome", "Nome deve ter no máximo 100 caracteres")
                .IsNotNullOrEmpty(cpf, "CPF", "CPF é obrigatório")
                .IsTrue(IsValidCPF(cpf), "CPF", "CPF inválido")
                .IsGreaterThan(dataNascimento, DateTime.MinValue, "DataNascimento", "Data de nascimento inválida")
                .IsLowerThan(dataNascimento, DateTime.UtcNow, "DataNascimento", "Data de nascimento não pode ser no futuro")
            );

            if (!string.IsNullOrWhiteSpace(email))
            {
                AddNotifications(new Contract<tPessoa>()
                    .Requires()
                    .IsEmail(email, "Email", "Email inválido")
                    .IsLowerThan(email.Length, 101, "Email", "Email deve ter no máximo 100 caracteres")
                );
            }

            if (!string.IsNullOrWhiteSpace(telefone))
            {
                AddNotifications(new Contract<tPessoa>()
                    .Requires()
                    .IsTrue(IsValidTelefone(telefone), "Telefone", "Telefone inválido")
                );
            }
        }

        public void AtualizarNome(string novoNome)
        {
            AddNotifications(new Contract<tPessoa>()
                .Requires()
                .IsNotNullOrEmpty(novoNome, "Nome", "Nome é obrigatório")
                .IsGreaterThan(novoNome.Length, 1, "Nome", "Nome deve ter pelo menos 2 caracteres")
                .IsLowerThan(novoNome.Length, 101, "Nome", "Nome deve ter no máximo 100 caracteres")
            );

            if (IsValid)
            {
                Nome = novoNome;
                SetLastUpdateDate();
            }
        }

        public void AtualizarEmail(string? novoEmail)
        {
            if (!string.IsNullOrWhiteSpace(novoEmail))
            {
                AddNotifications(new Contract<tPessoa>()
                    .Requires()
                    .IsEmail(novoEmail, "Email", "Email inválido")
                    .IsLowerThan(novoEmail.Length, 101, "Email", "Email deve ter no máximo 100 caracteres")
                );
            }

            if (IsValid)
            {
                Email = novoEmail;
                SetLastUpdateDate();
            }
        }

        public void AtualizarTelefone(string? novoTelefone)
        {
            if (!string.IsNullOrWhiteSpace(novoTelefone))
            {
                AddNotifications(new Contract<tPessoa>()
                    .Requires()
                    .IsTrue(IsValidTelefone(novoTelefone), "Telefone", "Telefone inválido")
                );
            }

            if (IsValid)
            {
                Telefone = novoTelefone;
                SetLastUpdateDate();
            }
        }

        public void AtualizarDataNascimento(DateTime novaData)
        {
            if (novaData > DateTime.UtcNow)
                AddNotification("DataNascimento", "Data de nascimento não pode ser no futuro");

            if (IsValid)
            {
                DataNascimento = novaData;
                SetLastUpdateDate();
            }
        }

        public void AtualizarNaturalidade(string? novaNaturalidade)
        {
            if (!string.IsNullOrWhiteSpace(novaNaturalidade) && novaNaturalidade.Length > 50)
                AddNotification("Naturalidade", "Naturalidade deve ter no máximo 50 caracteres");

            if (IsValid)
            {
                Naturalidade = novaNaturalidade;
                SetLastUpdateDate();
            }
        }

        public void AtualizarNacionalidade(string? novaNacionalidade)
        {
            if (!string.IsNullOrWhiteSpace(novaNacionalidade) && novaNacionalidade.Length > 50)
                AddNotification("Nacionalidade", "Nacionalidade deve ter no máximo 50 caracteres");

            if (IsValid)
            {
                Nacionalidade = novaNacionalidade;
                SetLastUpdateDate();
            }
        }

        public void AtualizarCPF(string novoCPF)
        {
            if (!IsValidCPF(novoCPF))
                AddNotification("CPF", "CPF inválido");

            if (IsValid)
            {
                CPF = novoCPF;
                SetLastUpdateDate();
            }
        }

        public void Ativar()
        {
            BitAtivo = true;
            SetLastUpdateDate();
        }

        public void Desativar()
        {
            BitAtivo = false;
            SetLastUpdateDate();
        }

        private static bool IsValidTelefone(string telefone)
        {
            var cleanPhone = Regex.Replace(telefone, @"[^\d]", "");
            return cleanPhone.Length >= 8 && cleanPhone.Length <= 11;
        }

        private static bool IsValidCPF(string cpf)
        {
            cpf = Regex.Replace(cpf, @"[^\d]", "");

            if (cpf.Length != 11 || new string(cpf[0], 11) == cpf)
                return false;

            int[] multiplicador1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCpf = cpf.Substring(0, 9);
            int soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

            int resto = soma % 11;
            resto = resto < 2 ? 0 : 11 - resto;

            string digito = resto.ToString();
            tempCpf += digito;
            soma = 0;

            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
            resto = resto < 2 ? 0 : 11 - resto;

            digito += resto.ToString();

            return cpf.EndsWith(digito);
        }
    }
}
