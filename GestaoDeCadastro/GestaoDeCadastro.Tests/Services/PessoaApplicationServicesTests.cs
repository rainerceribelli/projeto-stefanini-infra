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
