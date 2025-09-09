using GestaoDeCadastro.Domain.Entities.Cadastro;
using GestaoDeCadastro.Infraestructure.Persistance.EntityFramework;
using GestaoDeCadastro.Infraestructure.Persistance.Repository;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GestaoDeCadastro.Tests.Infrastructure.Repositories
{
    public class EnderecoRepositoryTests : IDisposable
    {
        private readonly GenericContext _context;
        private readonly EnderecoRepository _repository;

        public EnderecoRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<GenericContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new GenericContext(options);
            _repository = new EnderecoRepository(_context);
        }

        [Fact]
        public async Task AddAsync_ComEnderecoValido_DeveAdicionarEndereco()
        {
            var endereco = new tEndereco("Rua das Flores", "123", "São Paulo", "SP", "01234567", 1, "Apto 45", "Centro");

            await _repository.AddAsync(endereco);
            await _context.SaveChangesAsync();

            var enderecoSalvo = await _context.Enderecos.FirstOrDefaultAsync(e => e.CEP == "01234567");
            Assert.NotNull(enderecoSalvo);
            Assert.Equal("Rua das Flores", enderecoSalvo.Logradouro);
        }

        [Fact]
        public async Task GetByIdAsync_ComIdExistente_DeveRetornarEndereco()
        {
            var endereco = new tEndereco("Rua das Flores", "123", "São Paulo", "SP", "01234567", 1);
            _context.Enderecos.Add(endereco);
            await _context.SaveChangesAsync();

            var enderecoEncontrado = await _repository.GetByIdAsync(endereco.Id);

            Assert.NotNull(enderecoEncontrado);
            Assert.Equal("Rua das Flores", enderecoEncontrado.Logradouro);
        }

        [Fact]
        public async Task GetByIdAsync_ComIdInexistente_DeveRetornarNull()
        {
            var enderecoEncontrado = await _repository.GetByIdAsync(999);

            Assert.Null(enderecoEncontrado);
        }

        [Fact]
        public async Task GetAll_DeveRetornarTodosOsEnderecos()
        {
            var endereco1 = new tEndereco("Rua das Flores", "123", "São Paulo", "SP", "01234567", 1);
            var endereco2 = new tEndereco("Avenida Paulista", "456", "São Paulo", "SP", "01310100", 2);
            
            _context.Enderecos.AddRange(endereco1, endereco2);
            await _context.SaveChangesAsync();

            var enderecos = _repository.GetAll().ToList();

            Assert.Equal(2, enderecos.Count);
        }

        [Fact]
        public async Task Update_ComEnderecoExistente_DeveAtualizarEndereco()
        {
            var endereco = new tEndereco("Rua das Flores", "123", "São Paulo", "SP", "01234567", 1);
            _context.Enderecos.Add(endereco);
            await _context.SaveChangesAsync();

            endereco.AtualizarEndereco("Avenida Paulista", "456", "São Paulo", "SP", "01310100", "Sala 12", "Bela Vista");
            _repository.Update(endereco);
            await _context.SaveChangesAsync();

            var enderecoAtualizado = await _context.Enderecos.FindAsync(endereco.Id);
            Assert.Equal("Avenida Paulista", enderecoAtualizado.Logradouro);
            Assert.Equal("456", enderecoAtualizado.Numero);
        }

        [Fact]
        public async Task Delete_ComEnderecoExistente_DeveRemoverEndereco()
        {
            var endereco = new tEndereco("Rua das Flores", "123", "São Paulo", "SP", "01234567", 1);
            _context.Enderecos.Add(endereco);
            await _context.SaveChangesAsync();

            _repository.Delete(endereco);
            await _context.SaveChangesAsync();

            var enderecoRemovido = await _context.Enderecos.FindAsync(endereco.Id);
            Assert.Null(enderecoRemovido);
        }


        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
