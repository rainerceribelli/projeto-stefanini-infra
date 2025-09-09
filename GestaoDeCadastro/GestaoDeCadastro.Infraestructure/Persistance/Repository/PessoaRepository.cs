using GestaoDeCadastro.Domain.Entities.Cadastro;
using GestaoDeCadastro.Infraestructure.Persistance.EntityFramework;

namespace GestaoDeCadastro.Infraestructure.Persistance.Repository
{
    public class PessoaRepository : GenericRepository<tPessoa, GenericContext>
    {
        public PessoaRepository(GenericContext context)
            : base(context)
        {
        }
    }
}
