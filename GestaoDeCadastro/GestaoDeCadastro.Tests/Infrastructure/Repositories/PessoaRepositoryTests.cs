using GestaoDeCadastro.Domain.Entities.Cadastro;
using GestaoDeCadastro.Infraestructure.Persistance.EntityFramework;
using GestaoDeCadastro.Infraestructure.Persistance.Repository;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GestaoDeCadastro.Tests.Infrastructure.Repositories
{
    public class PessoaRepositoryTests : IDisposable
    {
        private readonly GenericContext _context;
        private readonly PessoaRepository _repository;

        public PessoaRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<GenericContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new GenericContext(options);
            _repository = new PessoaRepository(_context);
        }

        [Fact]
        public async Task AddAsync_ComPessoaValida_DeveAdicionarPessoa()
        {
            var pessoa = new tPessoa("João Silva", "11144477735", new DateTime(1990, 1, 1), "joao@email.com");

            await _repository.AddAsync(pessoa);
            await _context.SaveChangesAsync();

            var pessoaSalva = await _context.Pessoas.FirstOrDefaultAsync(p => p.CPF == "11144477735");
            Assert.NotNull(pessoaSalva);
            Assert.Equal("João Silva", pessoaSalva.Nome);
        }

        [Fact]
        public async Task GetByIdAsync_ComIdExistente_DeveRetornarPessoa()
        {
            var pessoa = new tPessoa("João Silva", "11144477735", new DateTime(1990, 1, 1));
            _context.Pessoas.Add(pessoa);
            await _context.SaveChangesAsync();

            var pessoaEncontrada = await _repository.GetByIdAsync(pessoa.Id);

            Assert.NotNull(pessoaEncontrada);
            Assert.Equal("João Silva", pessoaEncontrada.Nome);
        }

        [Fact]
        public async Task GetByIdAsync_ComIdInexistente_DeveRetornarNull()
        {
            var pessoaEncontrada = await _repository.GetByIdAsync(999);

            Assert.Null(pessoaEncontrada);
        }

        [Fact]
        public async Task GetAll_DeveRetornarTodasAsPessoas()
        {
            var pessoa1 = new tPessoa("João Silva", "11144477735", new DateTime(1990, 1, 1));
            var pessoa2 = new tPessoa("Maria Santos", "12345678909", new DateTime(1985, 5, 15));
            
            _context.Pessoas.AddRange(pessoa1, pessoa2);
            await _context.SaveChangesAsync();

            var pessoas = _repository.GetAll().ToList();

            Assert.Equal(2, pessoas.Count);
        }

        [Fact]
        public async Task Update_ComPessoaExistente_DeveAtualizarPessoa()
        {
            var pessoa = new tPessoa("João Silva", "11144477735", new DateTime(1990, 1, 1));
            _context.Pessoas.Add(pessoa);
            await _context.SaveChangesAsync();

            pessoa.AtualizarNome("João Santos");
            _repository.Update(pessoa);
            await _context.SaveChangesAsync();

            var pessoaAtualizada = await _context.Pessoas.FindAsync(pessoa.Id);
            Assert.Equal("João Santos", pessoaAtualizada.Nome);
        }

        [Fact]
        public async Task Delete_ComPessoaExistente_DeveRemoverPessoa()
        {
            var pessoa = new tPessoa("João Silva", "11144477735", new DateTime(1990, 1, 1));
            _context.Pessoas.Add(pessoa);
            await _context.SaveChangesAsync();

            _repository.Delete(pessoa);
            await _context.SaveChangesAsync();

            var pessoaRemovida = await _context.Pessoas.FindAsync(pessoa.Id);
            Assert.Null(pessoaRemovida);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
