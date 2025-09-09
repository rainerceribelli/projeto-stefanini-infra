using System.ComponentModel.DataAnnotations;

namespace GestaoDeCadastro.Crosscutting.DTO.Cadastro.V2
{
    public class UpdateEnderecoDTO
    {
        [Required(ErrorMessage = "ID do endereço é obrigatório")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Rua é obrigatória")]
        [MaxLength(100, ErrorMessage = "Rua deve ter no máximo 100 caracteres")]
        public string Rua { get; set; } = string.Empty;

        [Required(ErrorMessage = "Número é obrigatório")]
        [MaxLength(10, ErrorMessage = "Número deve ter no máximo 10 caracteres")]
        public string Numero { get; set; } = string.Empty;

        public string Complemento { get; set; } = string.Empty;

        public string Bairro { get; set; } = string.Empty;

        [Required(ErrorMessage = "Cidade é obrigatória")]
        [MaxLength(50, ErrorMessage = "Cidade deve ter no máximo 50 caracteres")]
        public string Cidade { get; set; } = string.Empty;

        [Required(ErrorMessage = "Estado é obrigatório")]
        [MaxLength(2, ErrorMessage = "Estado deve ter exatamente 2 caracteres")]
        [MinLength(2, ErrorMessage = "Estado deve ter exatamente 2 caracteres")]
        public string Estado { get; set; } = string.Empty;

        [Required(ErrorMessage = "CEP é obrigatório")]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "CEP deve ter exatamente 8 dígitos")]
        public string CEP { get; set; } = string.Empty;
    }
}
