using System.ComponentModel.DataAnnotations;

namespace GestaoDeCadastro.Crosscutting.DTO.Auth
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Username é obrigatório")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password é obrigatório")]
        public string Password { get; set; }
    }
}
