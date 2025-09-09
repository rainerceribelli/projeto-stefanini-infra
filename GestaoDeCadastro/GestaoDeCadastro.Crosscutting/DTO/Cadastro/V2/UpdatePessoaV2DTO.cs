using System.ComponentModel.DataAnnotations;

namespace GestaoDeCadastro.Crosscutting.DTO.Cadastro.V2
{
    public class UpdatePessoaV2DTO
    {
        [Required(ErrorMessage = "ID da pessoa é obrigatório")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Informar o nome da pessoa!")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Email inválido")]
        [StringLength(100, ErrorMessage = "O email deve ter no máximo 100 caracteres")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Data de nascimento é obrigatória")]
        public DateTime DataNascimento { get; set; }

        [StringLength(50, ErrorMessage = "A naturalidade deve ter no máximo 50 caracteres")]
        public string? Naturalidade { get; set; }

        [StringLength(50, ErrorMessage = "A nacionalidade deve ter no máximo 50 caracteres")]
        public string? Nacionalidade { get; set; }

        [Required(ErrorMessage = "CPF é obrigatório")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "CPF deve ter exatamente 11 dígitos")]
        public string CPF { get; set; } = string.Empty;

        public string? Telefone { get; set; }

        public bool BitAtivo { get; set; } = true;

        [Required(ErrorMessage = "Endereço é obrigatório na versão 2")]
        public UpdateEnderecoDTO Endereco { get; set; } = new UpdateEnderecoDTO();
    }
}
