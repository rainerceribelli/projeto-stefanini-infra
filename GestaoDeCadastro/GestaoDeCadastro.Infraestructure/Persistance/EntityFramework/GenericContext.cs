using GestaoDeCadastro.Domain.Entities.Cadastro;
using Microsoft.EntityFrameworkCore;

namespace GestaoDeCadastro.Infraestructure.Persistance.EntityFramework
{
    public class GenericContext : DbContext
    {
        public GenericContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<tPessoa> Pessoas { get; set; }
        public DbSet<tEndereco> Enderecos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Ignore<GestaoDeCadastro.Domain.Entities.Entity>();

            modelBuilder.Ignore<Flunt.Notifications.Notification>();

            modelBuilder.Entity<tPessoa>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nome).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).HasMaxLength(100);
                entity.Property(e => e.DataNascimento).IsRequired();
                entity.Property(e => e.Naturalidade).HasMaxLength(50);
                entity.Property(e => e.Nacionalidade).HasMaxLength(50);
                entity.Property(e => e.CPF).IsRequired().HasMaxLength(11);
                entity.Property(e => e.DataCadastro).IsRequired();
                entity.HasIndex(e => e.CPF).IsUnique();

                entity.HasOne(e => e.Endereco)
                      .WithOne(e => e.Pessoa)
                      .HasForeignKey<tEndereco>(e => e.PessoaId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<tEndereco>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Logradouro).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Numero).IsRequired().HasMaxLength(10);
                entity.Property(e => e.Cidade).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Estado).IsRequired().HasMaxLength(2);
                entity.Property(e => e.CEP).IsRequired().HasMaxLength(8);
                entity.Property(e => e.DataCadastro).IsRequired();
            });

        }
    }
}
