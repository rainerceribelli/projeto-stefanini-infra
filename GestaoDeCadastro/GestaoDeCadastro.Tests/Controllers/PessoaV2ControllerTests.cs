using GestaoDeCadastro.Crosscutting.DTO.Cadastro.V2;
using GestaoDeCadastro.Interface.Controllers;
using GestaoDeCadastro.Services.ApplicationServices.Cadastro;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace GestaoDeCadastro.Tests.Controllers
{
    public class PessoaV2ControllerTests : IDisposable
    {
        private readonly PessoaApplicationServices _service;
        private readonly PessoaV2Controller _controller;
        private readonly GestaoDeCadastro.Infraestructure.Persistance.EntityFramework.GenericContext _context;

        public PessoaV2ControllerTests()
        {
            var options = new DbContextOptionsBuilder<GestaoDeCadastro.Infraestructure.Persistance.EntityFramework.GenericContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new GestaoDeCadastro.Infraestructure.Persistance.EntityFramework.GenericContext(options);
            var pessoaRepository = new GestaoDeCadastro.Infraestructure.Persistance.Repository.PessoaRepository(_context);
            var enderecoRepository = new GestaoDeCadastro.Infraestructure.Persistance.Repository.EnderecoRepository(_context);
            
            var serviceProvider = new ServiceCollection()
                .AddScoped<GestaoDeCadastro.Infraestructure.Persistance.Repository.PessoaRepository>(_ => pessoaRepository)
                .AddScoped<GestaoDeCadastro.Infraestructure.Persistance.Repository.EnderecoRepository>(_ => enderecoRepository)
                .BuildServiceProvider();
            
            var unitOfWork = new GestaoDeCadastro.Infraestructure.Persistance.UnitOfWork.Cadastro.PessoaUnitOfWork(_context, serviceProvider);
            _service = new PessoaApplicationServices(unitOfWork);
            _controller = new PessoaV2Controller(_service);
        }

        [Fact]
        public async Task GetListPessoas_DeveRetornarOkComListaDePessoas()
        {
            var createPessoaDto = new CreatePessoaV2DTO
            {
                Nome = "João Silva",
                CPF = "11144477735",
                DataNascimento = new DateTime(1990, 1, 1),
                Email = "joao@email.com",
                Endereco = new CreateEnderecoDTO
                {
                    Logradouro = "Rua das Flores",
                    Numero = "123",
                    Cidade = "São Paulo",
                    Estado = "SP",
                    CEP = "01234567"
                }
            };
            await _service.CreatePessoaV2(createPessoaDto);

            var result = await _controller.GetListPessoas();

            var actionResult = Assert.IsType<ActionResult<List<PessoaV2DTO>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnValue = Assert.IsType<List<PessoaV2DTO>>(okResult.Value);
            Assert.Single(returnValue);
        }

        [Fact]
        public async Task CreatePessoa_ComDadosValidos_DeveRetornarOk()
        {
            var createPessoaDto = new CreatePessoaV2DTO
            {
                Nome = "João Silva",
                CPF = "11144477735",
                DataNascimento = new DateTime(1990, 1, 1),
                Email = "joao@email.com",
                Endereco = new CreateEnderecoDTO
                {
                    Logradouro = "Rua das Flores",
                    Numero = "123",
                    Cidade = "São Paulo",
                    Estado = "SP",
                    CEP = "01234567"
                }
            };

            var result = await _controller.CreatePessoa(createPessoaDto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Pessoa criada com sucesso", okResult.Value);
        }

        [Fact]
        public async Task CreatePessoa_SemEndereco_DeveRetornarBadRequest()
        {
            var createPessoaDto = new CreatePessoaV2DTO
            {
                Nome = "João Silva",
                CPF = "11144477735",
                DataNascimento = new DateTime(1990, 1, 1),
                Endereco = null
            };

            var result = await _controller.CreatePessoa(createPessoaDto);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Endereço é obrigatório na versão 2 da API", badRequestResult.Value);
        }

        [Fact]
        public async Task CreatePessoa_ComCPFInvalido_DeveRetornarBadRequest()
        {
            var createPessoaDto = new CreatePessoaV2DTO
            {
                Nome = "João Silva",
                CPF = "12345678900",
                DataNascimento = new DateTime(1990, 1, 1),
                Endereco = new CreateEnderecoDTO
                {
                    Logradouro = "Rua das Flores",
                    Numero = "123",
                    Cidade = "São Paulo",
                    Estado = "SP",
                    CEP = "01234567"
                }
            };

            var result = await _controller.CreatePessoa(createPessoaDto);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Contains("CPF inválido", badRequestResult.Value.ToString());
        }

        [Fact]
        public async Task UpdatePessoa_ComDadosValidos_DeveRetornarOk()
        {
            var createPessoaDto = new CreatePessoaV2DTO
            {
                Nome = "João Silva",
                CPF = "11144477735",
                DataNascimento = new DateTime(1990, 1, 1),
                Email = "joao@email.com",
                Endereco = new CreateEnderecoDTO
                {
                    Logradouro = "Rua das Flores",
                    Numero = "123",
                    Cidade = "São Paulo",
                    Estado = "SP",
                    CEP = "01234567"
                }
            };
            await _service.CreatePessoaV2(createPessoaDto);

            var pessoas = await _service.GetListPessoasV2();
            var pessoaId = pessoas.First().Id;

            var updatePessoaDto = new UpdatePessoaV2DTO
            {
                Id = pessoaId,
                Nome = "João Santos",
                CPF = "11144477735",
                DataNascimento = new DateTime(1990, 1, 1),
                Email = "joao.santos@email.com",
                Endereco = new UpdateEnderecoDTO
                {
                    Id = 1,
                    Rua = "Avenida Paulista",
                    Numero = "456",
                    Cidade = "São Paulo",
                    Estado = "SP",
                    CEP = "01310100"
                }
            };

            var result = await _controller.UpdatePessoa(updatePessoaDto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Pessoa atualizada com sucesso", okResult.Value);
        }

        [Fact]
        public async Task UpdatePessoa_SemEndereco_DeveRetornarBadRequest()
        {
            var updatePessoaDto = new UpdatePessoaV2DTO
            {
                Id = 1,
                Nome = "João Santos",
                CPF = "11144477735",
                DataNascimento = new DateTime(1990, 1, 1),
                Endereco = null
            };

            var result = await _controller.UpdatePessoa(updatePessoaDto);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Endereço é obrigatório na versão 2 da API", badRequestResult.Value);
        }

        [Fact]
        public async Task UpdatePessoa_ComIdInexistente_DeveRetornarBadRequest()
        {
            var updatePessoaDto = new UpdatePessoaV2DTO
            {
                Id = 999,
                Nome = "João Santos",
                CPF = "11144477735",
                DataNascimento = new DateTime(1990, 1, 1),
                Endereco = new UpdateEnderecoDTO
                {
                    Id = 1,
                    Rua = "Avenida Paulista",
                    Numero = "456",
                    Cidade = "São Paulo",
                    Estado = "SP",
                    CEP = "01310100"
                }
            };

            var result = await _controller.UpdatePessoa(updatePessoaDto);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Pessoa não encontrada", badRequestResult.Value);
        }

        [Fact]
        public async Task DeletePessoa_ComIdExistente_DeveRetornarOk()
        {
            var createPessoaDto = new CreatePessoaV2DTO
            {
                Nome = "João Silva",
                CPF = "11144477735",
                DataNascimento = new DateTime(1990, 1, 1),
                Email = "joao@email.com",
                Endereco = new CreateEnderecoDTO
                {
                    Logradouro = "Rua das Flores",
                    Numero = "123",
                    Cidade = "São Paulo",
                    Estado = "SP",
                    CEP = "01234567"
                }
            };
            await _service.CreatePessoaV2(createPessoaDto);

            var pessoas = await _service.GetListPessoasV2();
            var pessoaId = pessoas.First().Id;

            var result = await _controller.DeletePessoa(pessoaId);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Pessoa excluída com sucesso", okResult.Value);
        }

        [Fact]
        public async Task DeletePessoa_ComIdInexistente_DeveRetornarBadRequest()
        {
            var id = 999;

            var result = await _controller.DeletePessoa(id);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Pessoa não encontrada", badRequestResult.Value);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}