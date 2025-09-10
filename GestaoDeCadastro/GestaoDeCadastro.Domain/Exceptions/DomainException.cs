namespace GestaoDeCadastro.Domain.Exceptions
{
    public class DomainException : Exception
    {
        public List<string> Errors { get; private set; }

        public DomainException(List<string> errors)
            : base("Erro(s) de validação de domínio")
        {
            Errors = errors;
        }
    }
}
