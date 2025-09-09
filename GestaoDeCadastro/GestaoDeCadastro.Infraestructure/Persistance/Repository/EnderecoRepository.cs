using GestaoDeCadastro.Domain.Entities.Cadastro;
using GestaoDeCadastro.Infraestructure.Persistance.EntityFramework;

namespace GestaoDeCadastro.Infraestructure.Persistance.Repository
{
    public class EnderecoRepository : GenericRepository<tEndereco, GenericContext>
    {
        public EnderecoRepository(GenericContext context)
            : base(context)
        {
        }
    }
}
