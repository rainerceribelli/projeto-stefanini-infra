using GestaoDeCadastro.Crosscutting.DTO.Cadastro.V1;
using GestaoDeCadastro.Crosscutting.DTO.Cadastro.V2;
using GestaoDeCadastro.Domain.Entities.Cadastro;
using GestaoDeCadastro.Infraestructure.Persistance.EntityFramework;
using GestaoDeCadastro.Infraestructure.Persistance.Repository;
using GestaoDeCadastro.Infraestructure.Persistance.UnitOfWork.Cadastro;
using GestaoDeCadastro.Services.ApplicationServices.Cadastro;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace GestaoDeCadastro.Tests.Services
{
    public class PessoaApplicationServicesTests : IDisposable
    {
        private readonly GenericContext _context;
        private readonly PessoaRepository _pessoaRepository;
        private readonly EnderecoRepository _enderecoRepository;
        private readonly PessoaUnitOfWork _unitOfWork;
        private readonly PessoaApplicationServices _service;

        public PessoaApplicationServicesTests()
        {
            var options = new DbContextOptionsBuilder<GenericContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new GenericContext(options);
            _pessoaRepository = new PessoaRepository(_context);
            _enderecoRepository = new EnderecoRepository(_context);
            var serviceProvider = new ServiceCollection()
                .AddScoped<PessoaRepository>(_ => _pessoaRepository)
                .AddScoped<EnderecoRepository>(_ => _enderecoRepository)
                .BuildServiceProvider();
            _unitOfWork = new PessoaUnitOfWork(_context, serviceProvider);
            _service = new PessoaApplicationServices(_unitOfWork);
        }

        [Fact]
        public async Task CreatePessoa_ComDadosValidos_DeveCriarPessoa()
        {
            var createPessoaDto = new CreatePessoaDTO
            {
                Nome = "João Silva",
                CPF = "11144477735",
                DataNascimento = new DateTime(1990, 1, 1),
                Email = "joao@email.com",
                Naturalidade = "São Paulo",
                Nacionalidade = "Brasileira"
            };

            await _service.CreatePessoa(createPessoaDto);

            var pessoaCriada = await _context.Pessoas.FirstOrDefaultAsync(p => p.CPF == "12345678901");
            Assert.NotNull(pessoaCriada);
            Assert.Equal("João Silva", pessoaCriada.Nome);
            Assert.Equal("joao@email.com", pessoaCriada.Email);
        }

        [Fact]
        public async Task CreatePessoa_ComCPFDuplicado_DeveLancarExcecao()
        {
            var pessoaExistente = new tPessoa("Maria Santos", "11144477735", new DateTime(1985, 5, 15));
            _context.Pessoas.Add(pessoaExistente);
            await _context.SaveChangesAsync();

            var createPessoaDto = new CreatePessoaDTO
            {
                Nome = "João Silva",
                CPF = "11144477735",
                DataNascimento = new DateTime(1990, 1, 1)
            };

            await Assert.ThrowsAsync<Exception>(async () => await _service.CreatePessoa(createPessoaDto));
        }

        [Fact]
        public async Task CreatePessoaV2_ComDadosValidos_DeveCriarPessoaComEndereco()
        {
            var createPessoaV2Dto = new CreatePessoaV2DTO
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
                    CEP = "01234567",
                    Complemento = "Apto 45",
                    Bairro = "Centro"
                }
            };

            await _service.CreatePessoaV2(createPessoaV2Dto);

            var pessoaCriada = await _context.Pessoas
                .Include(p => p.Endereco)
                .FirstOrDefaultAsync(p => p.CPF == "12345678901");
            
            Assert.NotNull(pessoaCriada);
            Assert.NotNull(pessoaCriada.Endereco);
            Assert.Equal("Rua das Flores", pessoaCriada.Endereco.Logradouro);
        }

        [Fact]
        public async Task CreatePessoaV2_SemEndereco_DeveLancarExcecao()
        {
            var createPessoaV2Dto = new CreatePessoaV2DTO
            {
                Nome = "João Silva",
                CPF = "11144477735",
                DataNascimento = new DateTime(1990, 1, 1),
                Endereco = null
            };

            await Assert.ThrowsAsync<Exception>(async () => await _service.CreatePessoaV2(createPessoaV2Dto));
        }

        [Fact]
        public async Task GetListPessoas_DeveRetornarListaDePessoas()
        {
            var pessoa1 = new tPessoa("João Silva", "11144477735", new DateTime(1990, 1, 1));
            var pessoa2 = new tPessoa("Maria Santos", "12345678909", new DateTime(1985, 5, 15));
            
            _context.Pessoas.AddRange(pessoa1, pessoa2);
            await _context.SaveChangesAsync();

            var pessoas = await _service.GetListPessoas();

            Assert.Equal(2, pessoas.Count);
        }

        [Fact]
        public async Task GetListPessoasV2_DeveRetornarListaDePessoasComEndereco()
        {
            var pessoa1 = new tPessoa("João Silva", "11144477735", new DateTime(1990, 1, 1));
            var pessoa2 = new tPessoa("Maria Santos", "12345678909", new DateTime(1985, 5, 15));
            
            _context.Pessoas.AddRange(pessoa1, pessoa2);
            await _context.SaveChangesAsync();

            var endereco1 = new tEndereco("Rua das Flores", "123", "São Paulo", "SP", "01234567", pessoa1.Id);
            var endereco2 = new tEndereco("Avenida Paulista", "456", "São Paulo", "SP", "01310100", pessoa2.Id);
            
            _context.Enderecos.AddRange(endereco1, endereco2);
            await _context.SaveChangesAsync();

            var pessoas = await _service.GetListPessoasV2();

            Assert.Equal(2, pessoas.Count);
            Assert.All(pessoas, p => Assert.NotNull(p.Endereco));
        }

        [Fact]
        public async Task UpdatePessoa_ComDadosValidos_DeveAtualizarPessoa()
        {
            var pessoa = new tPessoa("João Silva", "12345678901", new DateTime(1990, 1, 1));
            _context.Pessoas.Add(pessoa);
            await _context.SaveChangesAsync();

            var updatePessoaDto = new UpdatePessoaDTO
            {
                Id = pessoa.Id,
                Nome = "João Santos",
                CPF = "11144477735",
                DataNascimento = new DateTime(1990, 1, 1),
                Email = "joao.santos@email.com"
            };

            await _service.UpdatePessoa(updatePessoaDto);

            var pessoaAtualizada = await _context.Pessoas.FindAsync(pessoa.Id);
            Assert.Equal("João Santos", pessoaAtualizada.Nome);
            Assert.Equal("joao.santos@email.com", pessoaAtualizada.Email);
        }

        [Fact]
        public async Task UpdatePessoa_ComIdInexistente_DeveLancarExcecao()
        {
            var updatePessoaDto = new UpdatePessoaDTO
            {
                Id = 999,
                Nome = "João Santos",
                CPF = "11144477735",
                DataNascimento = new DateTime(1990, 1, 1)
            };

            await Assert.ThrowsAsync<Exception>(async () => await _service.UpdatePessoa(updatePessoaDto));
        }

        [Fact]
        public async Task UpdatePessoaV2_ComDadosValidos_DeveAtualizarPessoaEEndereco()
        {
            var pessoa = new tPessoa("João Silva", "12345678901", new DateTime(1990, 1, 1));
            _context.Pessoas.Add(pessoa);
            await _context.SaveChangesAsync();

            var endereco = new tEndereco("Rua das Flores", "123", "São Paulo", "SP", "01234567", pessoa.Id);
            _context.Enderecos.Add(endereco);
            await _context.SaveChangesAsync();

            var updatePessoaV2Dto = new UpdatePessoaV2DTO
            {
                Id = pessoa.Id,
                Nome = "João Santos",
                CPF = "11144477735",
                DataNascimento = new DateTime(1990, 1, 1),
                Endereco = new UpdateEnderecoDTO
                {
                    Id = endereco.Id,
                    Rua = "Avenida Paulista",
                    Numero = "456",
                    Cidade = "São Paulo",
                    Estado = "SP",
                    CEP = "01310100"
                }
            };

            await _service.UpdatePessoaV2(updatePessoaV2Dto);

            var pessoaAtualizada = await _context.Pessoas
                .Include(p => p.Endereco)
                .FirstOrDefaultAsync(p => p.Id == pessoa.Id);
            
            Assert.Equal("João Santos", pessoaAtualizada.Nome);
            Assert.Equal("Avenida Paulista", pessoaAtualizada.Endereco.Logradouro);
        }

        [Fact]
        public async Task DeletePessoa_ComIdExistente_DeveRemoverPessoa()
        {
            var pessoa = new tPessoa("João Silva", "12345678901", new DateTime(1990, 1, 1));
            _context.Pessoas.Add(pessoa);
            await _context.SaveChangesAsync();

            await _service.DeletePessoa(pessoa.Id);

            var pessoaRemovida = await _context.Pessoas.FindAsync(pessoa.Id);
            Assert.Null(pessoaRemovida);
        }

        [Fact]
        public async Task DeletePessoa_ComIdInexistente_DeveLancarExcecao()
        {
            await Assert.ThrowsAsync<Exception>(async () => await _service.DeletePessoa(999));
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
