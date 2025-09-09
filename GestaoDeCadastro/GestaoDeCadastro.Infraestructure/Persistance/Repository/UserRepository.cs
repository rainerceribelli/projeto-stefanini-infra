using GestaoDeCadastro.Domain.Entities.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoDeCadastro.Infraestructure.Persistance.Repository
{
    public static class UserRepository
    {
        public static User Get(string username, string password)
        {
            var users = new List<User>
            {
                new() { Id = 1, UserName = "admin", Password = "admin123", Role = "admin"},
                new() { Id = 2, UserName = "teste", Password = "teste123", Role = "teste" }
            };


            return users.
                FirstOrDefault(x =>
                    x.UserName == username
                    && x.Password == password);
        }
    }
}
