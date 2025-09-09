using Flunt.Notifications;

namespace GestaoDeCadastro.Domain.Entities
{
    public abstract class Entity : Notifiable<Notification>
    {
        public Entity()
        {
            Id = Guid.NewGuid();
            CreateDate = DateTime.UtcNow;
            LastUpdateDate = DateTime.UtcNow;
        }

        public Guid Id { get; private set; }
        public DateTime CreateDate { get; private set; }
        public DateTime LastUpdateDate { get; private set; }

        protected void SetLastUpdateDate()
        {
            LastUpdateDate = DateTime.UtcNow;
        }
    }
}
