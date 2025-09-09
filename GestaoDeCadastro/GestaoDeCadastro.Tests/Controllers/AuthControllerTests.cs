using GestaoDeCadastro.Interface.Controllers;
using GestaoDeCadastro.Infraestructure.Persistance.Repository;
using GestaoDeCadastro.Services.ApplicationServices.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GestaoDeCadastro.Tests.Controllers
{
    public class AuthControllerTests : IDisposable
    {
        private readonly AuthController _controller;
        private readonly TokenService _tokenService;

        public AuthControllerTests()
        {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    {"Jwt:Key", "MinhaChaveSecretaSuperSegura123456789"},
                    {"Jwt:Issuer", "GestaoDeCadastro"},
                    {"Jwt:Audience", "GestaoDeCadastroUsers"}
                })
                .Build();

            _tokenService = new TokenService(configuration);
            _controller = new AuthController(_tokenService);
        }

        [Fact]
        public async Task AuthenticateAsync_ComCredenciaisValidas_DeveRetornarToken()
        {
            var loginRequest = new GestaoDeCadastro.Domain.Entities.Auth.User 
            { 
                Id = 1,
                UserName = "admin", 
                Password = "admin123",
                Role = "admin"
            };

            var result = await _controller.AuthenticateAsync(loginRequest);

            var actionResult = Assert.IsType<ActionResult<dynamic>>(result);
            Assert.NotNull(actionResult.Value);
        }

        [Fact]
        public async Task AuthenticateAsync_ComUsuarioAdmin_DeveRetornarToken()
        {
            var loginRequest = new GestaoDeCadastro.Domain.Entities.Auth.User 
            { 
                Id = 1,
                UserName = "admin", 
                Password = "admin123",
                Role = "admin"
            };

            var result = await _controller.AuthenticateAsync(loginRequest);

            var actionResult = Assert.IsType<ActionResult<dynamic>>(result);
            Assert.NotNull(actionResult.Value);
        }

        [Fact]
        public async Task AuthenticateAsync_ComUsuarioTeste_DeveRetornarToken()
        {
            var loginRequest = new GestaoDeCadastro.Domain.Entities.Auth.User 
            { 
                Id = 2,
                UserName = "teste", 
                Password = "teste123",
                Role = "teste"
            };

            var result = await _controller.AuthenticateAsync(loginRequest);

            var actionResult = Assert.IsType<ActionResult<dynamic>>(result);
            Assert.NotNull(actionResult.Value);
        }

        [Fact]
        public async Task AuthenticateAsync_ComCredenciaisInvalidas_DeveRetornarNotFound()
        {
            var loginRequest = new GestaoDeCadastro.Domain.Entities.Auth.User 
            { 
                Id = 3,
                UserName = "usuario_inexistente", 
                Password = "senha_errada",
                Role = "user"
            };

            var result = await _controller.AuthenticateAsync(loginRequest);

            var actionResult = Assert.IsType<ActionResult<dynamic>>(result);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            Assert.NotNull(notFoundResult.Value);
        }

        public void Dispose()
        {
        }
    }
}