using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoDeCadastro.Crosscutting.DTO.Cadastro.V1
{
    public class CreatePessoaDTO
    {
        [Required(ErrorMessage = "Informar o nome da pessoa!")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "CPF é obrigatório")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "CPF deve ter exatamente 11 dígitos")]
        public string CPF { get; set; } = string.Empty;

        [Required(ErrorMessage = "Data de nascimento é obrigatória")]
        public DateTime DataNascimento { get; set; }

        [EmailAddress(ErrorMessage = "Email inválido")]
        [StringLength(100, ErrorMessage = "O email deve ter no máximo 100 caracteres")]
        public string? Email { get; set; }

        public string? Telefone { get; set; }

        [StringLength(50, ErrorMessage = "A naturalidade deve ter no máximo 50 caracteres")]
        public string? Naturalidade { get; set; }

        [StringLength(50, ErrorMessage = "A nacionalidade deve ter no máximo 50 caracteres")]
        public string? Nacionalidade { get; set; }

        public bool BitAtivo { get; set; } = true;
    }
}
