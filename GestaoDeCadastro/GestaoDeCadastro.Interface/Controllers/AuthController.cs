using GestaoDeCadastro.Domain.Entities.Auth;
using GestaoDeCadastro.Infraestructure.Persistance.Repository;
using GestaoDeCadastro.Services.ApplicationServices.Auth;
using Microsoft.AspNetCore.Mvc;

namespace GestaoDeCadastro.Interface.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly TokenService _tokenService;

        public AuthController(TokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<dynamic>> AuthenticateAsync([FromBody] User model)
        {
            var user = UserRepository.Get(model.UserName, model.Password);

            if (user == null)
                return NotFound(new { message = "Usuário ou senha inválidos" });

            var token = _tokenService.GenerateToken(user);

            user.Password = "";

            return new
            {
                user,
                token
            };
        }
    }

}
