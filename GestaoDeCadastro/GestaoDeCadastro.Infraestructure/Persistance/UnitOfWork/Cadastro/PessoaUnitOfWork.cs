using GestaoDeCadastro.Infraestructure.Persistance.EntityFramework;
using GestaoDeCadastro.Infraestructure.Persistance.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace GestaoDeCadastro.Infraestructure.Persistance.UnitOfWork.Cadastro
{
    public class PessoaUnitOfWork : GenericUnitOfWork
    {
        public PessoaRepository PessoaRepository => _serviceProvider.GetService<PessoaRepository>();

        public EnderecoRepository EnderecoRepository => _serviceProvider.GetService<EnderecoRepository>();

        public PessoaUnitOfWork(GenericContext context, IServiceProvider serviceProvider)
            : base(context, serviceProvider)
        {
        }
    }
}
