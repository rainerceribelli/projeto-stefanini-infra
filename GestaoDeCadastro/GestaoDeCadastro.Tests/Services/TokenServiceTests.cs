using GestaoDeCadastro.Services.ApplicationServices.Auth;
using GestaoDeCadastro.Domain.Entities.Auth;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace GestaoDeCadastro.Tests.Services
{
    public class TokenServiceTests
    {
        private readonly TokenService _tokenService;

        public TokenServiceTests()
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
        }

        [Fact]
        public void GenerateToken_ComUsuarioValido_DeveGerarTokenValido()
        {
            var user = new User 
            { 
                Id = 1,
                UserName = "admin", 
                Password = "admin123",
                Role = "admin"
            };

            var token = _tokenService.GenerateToken(user);

            Assert.NotNull(token);
            Assert.NotEmpty(token);
        }

        [Fact]
        public void GenerateToken_ComUsuarioDiferente_DeveGerarTokenComDadosCorretos()
        {
            var user = new User 
            { 
                Id = 2,
                UserName = "teste", 
                Password = "teste123",
                Role = "teste"
            };

            var token = _tokenService.GenerateToken(user);

            Assert.NotNull(token);
            Assert.NotEmpty(token);
        }

        [Fact]
        public void GenerateToken_ComUsuarioNull_DeveLancarExcecao()
        {
            Assert.Throws<NullReferenceException>(() => _tokenService.GenerateToken(null!));
        }

        [Fact]
        public void GenerateToken_ComConfiguracaoValida_DeveGerarTokenComClaimsCorretas()
        {
            var user = new User 
            { 
                Id = 1,
                UserName = "admin", 
                Password = "admin123",
                Role = "admin"
            };

            var token = _tokenService.GenerateToken(user);

            Assert.NotNull(token);
            Assert.NotEmpty(token);
            var parts = token.Split('.');
            Assert.Equal(3, parts.Length);
        }
    }
}