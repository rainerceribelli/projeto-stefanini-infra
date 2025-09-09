using System.ComponentModel.DataAnnotations;

namespace GestaoDeCadastro.Crosscutting.DTO.Cadastro.V2
{
    public class CreateEnderecoDTO
    {
        [Required(ErrorMessage = "Rua é obrigatória")]
        [MaxLength(100, ErrorMessage = "Rua deve ter no máximo 100 caracteres")]
        public string Logradouro { get; set; } = string.Empty;

        [Required(ErrorMessage = "Número é obrigatório")]
        [MaxLength(10, ErrorMessage = "Número deve ter no máximo 10 caracteres")]
        public string Numero { get; set; } = string.Empty;

        [Required(ErrorMessage = "Cidade é obrigatória")]
        [MaxLength(50, ErrorMessage = "Cidade deve ter no máximo 50 caracteres")]
        public string Cidade { get; set; } = string.Empty;

        [Required(ErrorMessage = "Estado é obrigatório")]
        [MaxLength(2, ErrorMessage = "Estado deve ter exatamente 2 caracteres")]
        [MinLength(2, ErrorMessage = "Estado deve ter exatamente 2 caracteres")]
        public string Estado { get; set; } = string.Empty;

        public string CEP { get; set; } = string.Empty;

        public string Complemento { get; set; }

        public string Bairro { get; set; }
    }
}
