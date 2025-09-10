using GestaoDeCadastro.Crosscutting.DTO.Cadastro.V1;
using GestaoDeCadastro.Crosscutting.DTO.Cadastro.V2;
using GestaoDeCadastro.Domain.Entities.Cadastro;
using GestaoDeCadastro.Infraestructure.Persistance.UnitOfWork.Cadastro;
using GestaoDeCadastro.Services.ApplicationServices.Base;
using Microsoft.EntityFrameworkCore;

namespace GestaoDeCadastro.Services.ApplicationServices.Cadastro
{
    public class PessoaApplicationServices : BaseApplicationServices
    {
        private readonly PessoaUnitOfWork _uow;

        public PessoaApplicationServices(PessoaUnitOfWork uow)
        {
            _uow = uow;
        }

        #region V1

        private IQueryable<PessoaDTO> GetQueryPessoaV1()
        {
            var query = _uow.PessoaRepository.GetAll()
                .Select(p => new PessoaDTO
                {
                    Id = p.Id,
                    Nome = p.Nome,
                    Email = p.Email,
                    DataNascimento = p.DataNascimento,
                    Naturalidade = p.Naturalidade,
                    Nacionalidade = p.Nacionalidade,
                    CPF = p.CPF,
                    Telefone = p.Telefone,
                    BitAtivo = p.BitAtivo,
                    DataCadastro = p.DataCadastro,
                    DataAtualizacao = p.DataAtualizacao,
                });

            return query;
        }

        public async Task<List<PessoaDTO>> GetListPessoas() => await GetQueryPessoaV1().ToListAsync();

        public async Task CreatePessoa(CreatePessoaDTO dto)
        {
            var pessoa = new tPessoa(
                dto.Nome,
                dto.CPF,
                dto.DataNascimento,
                dto.Email,
                dto.Naturalidade,
                dto.Nacionalidade,
                dto.Telefone
            );

            if (!pessoa.IsValid)
                throw new ArgumentException(string.Join(", ", pessoa.Notifications.Select(n => n.Message)));

            _uow.PessoaRepository.Add(pessoa);
            await _uow.CommitAsync();
        }

        public async Task UpdatePessoa(UpdatePessoaDTO dto)
        {
            var pessoa = await _uow.PessoaRepository.GetByIdAsync(dto.Id);
            if (pessoa == null)
                throw new Exception("Pessoa não encontrada");

            await ValidaPessoaUpdateV1(dto);

            pessoa.AtualizarNome(dto.Nome);
            pessoa.AtualizarEmail(dto.Email);
            pessoa.AtualizarTelefone(dto.Telefone);
            pessoa.AtualizarDataNascimento(dto.DataNascimento);
            pessoa.AtualizarNaturalidade(dto.Naturalidade);
            pessoa.AtualizarNacionalidade(dto.Nacionalidade);
            pessoa.AtualizarCPF(dto.CPF);

            if (dto.BitAtivo)
                pessoa.Ativar();
            else
                pessoa.Desativar();

            if (!pessoa.IsValid)
                throw new ArgumentException(string.Join(", ", pessoa.Notifications.Select(n => n.Message)));

            _uow.PessoaRepository.Update(pessoa);
            await _uow.CommitAsync();
        }

        private async Task ValidaPessoaUpdateV1(UpdatePessoaDTO dto)
        {
            var existente = await _uow.PessoaRepository.GetAll()
                .FirstOrDefaultAsync(p => p.CPF == dto.CPF && p.Id != dto.Id);

            if (existente != null)
                throw new Exception("CPF já cadastrado para outra pessoa");
        }

        #endregion

        #region V2

        private IQueryable<PessoaV2DTO> GetQueryPessoaV2()
        {
            var query = from pessoa in _uow.PessoaRepository.GetAll()
                        join endereco in _uow.EnderecoRepository.GetAll() on pessoa.Id equals endereco.PessoaId
                        select new PessoaV2DTO
                        {
                            Id = pessoa.Id,
                            Nome = pessoa.Nome,
                            Email = pessoa.Email,
                            DataNascimento = pessoa.DataNascimento,
                            Naturalidade = pessoa.Naturalidade,
                            Nacionalidade = pessoa.Nacionalidade,
                            CPF = pessoa.CPF,
                            Telefone = pessoa.Telefone,
                            BitAtivo = pessoa.BitAtivo,
                            DataCadastro = pessoa.DataCadastro,
                            DataAtualizacao = pessoa.DataAtualizacao,
                            EnderecoCompleto = endereco.ObterEnderecoCompleto(),
                            Logradouro = endereco.Logradouro,
                            Numero = endereco.Numero,
                            Complemento = endereco.Complemento,
                            Bairro = endereco.Bairro,
                            Cidade = endereco.Cidade,
                            Estado = endereco.Estado,
                            CEP = endereco.CEP
                        };

            return query;
        }

        public async Task<List<PessoaV2DTO>> GetListPessoasV2() => await GetQueryPessoaV2().ToListAsync();

        public async Task CreatePessoaV2(CreatePessoaV2DTO dto)
        {
            if (dto.Endereco == null)
                throw new ArgumentException("Endereço é obrigatório na versão 2 da API");

            var pessoa = new tPessoa(
                dto.Nome,
                dto.CPF,
                dto.DataNascimento,
                dto.Email,
                dto.Naturalidade,
                dto.Nacionalidade,
                dto.Telefone
            );

            if (!pessoa.IsValid)
                throw new ArgumentException(string.Join(", ", pessoa.Notifications.Select(n => n.Message)));

            _uow.PessoaRepository.Add(pessoa);

            var endereco = new tEndereco(
                dto.Endereco.Logradouro,
                dto.Endereco.Numero,
                dto.Endereco.Cidade,
                dto.Endereco.Estado,
                dto.Endereco.CEP,
                pessoa.Id,
                dto.Endereco.Complemento,
                dto.Endereco.Bairro
            );

            if (!endereco.IsValid)
                throw new ArgumentException(string.Join(", ", endereco.Notifications.Select(n => n.Message)));

            await _uow.EnderecoRepository.AddAsync(endereco);
            await _uow.CommitAsync();
        }

        public async Task UpdatePessoaV2(UpdatePessoaV2DTO dto)
        {
            if (dto.Endereco == null)
                throw new ArgumentException("Endereço é obrigatório na versão 2 da API");

            var pessoa = await _uow.PessoaRepository.GetByIdAsync(dto.Id);
            if (pessoa == null)
                throw new Exception("Pessoa não encontrada");

            await ValidaPessoaUpdateV2(dto);

            pessoa.AtualizarNome(dto.Nome);
            pessoa.AtualizarEmail(dto.Email);
            pessoa.AtualizarTelefone(dto.Telefone);
            pessoa.AtualizarDataNascimento(dto.DataNascimento);
            pessoa.AtualizarNaturalidade(dto.Naturalidade);
            pessoa.AtualizarNacionalidade(dto.Nacionalidade);
            pessoa.AtualizarCPF(dto.CPF);

            if (dto.BitAtivo)
                pessoa.Ativar();
            else
                pessoa.Desativar();

            if (!pessoa.IsValid)
                throw new ArgumentException(string.Join(", ", pessoa.Notifications.Select(n => n.Message)));

            _uow.PessoaRepository.Update(pessoa);

            if (dto.Endereco != null)
            {
                var endereco = await _uow.EnderecoRepository.GetAll()
                    .FirstOrDefaultAsync(e => e.PessoaId == dto.Id);

                if (endereco != null)
                {
                    endereco.AtualizarEndereco(
                        dto.Endereco.Rua,
                        dto.Endereco.Numero,
                        dto.Endereco.Cidade,
                        dto.Endereco.Estado,
                        dto.Endereco.CEP,
                        dto.Endereco.Complemento,
                        dto.Endereco.Bairro
                    );

                    if (!endereco.IsValid)
                        throw new ArgumentException(string.Join(", ", endereco.Notifications.Select(n => n.Message)));

                    _uow.EnderecoRepository.Update(endereco);
                }
            }

            await _uow.CommitAsync();
        }

        private async Task ValidaPessoaUpdateV2(UpdatePessoaV2DTO dto)
        {
            var existente = await _uow.PessoaRepository.GetAll()
                .FirstOrDefaultAsync(p => p.CPF == dto.CPF && p.Id != dto.Id);

            if (existente != null)
                throw new Exception("CPF já cadastrado para outra pessoa");
        }

        #endregion

        #region Comum

        public async Task DeletePessoa(int id)
        {
            var pessoa = await _uow.PessoaRepository.GetByIdAsync(id);
            if (pessoa == null)
                throw new Exception("Pessoa não encontrada");

            _uow.PessoaRepository.Delete(pessoa);
            await _uow.CommitAsync();
        }

        #endregion
    }
}
