using Domain.Entities;
using Infrastructure.EntityFramework;
using Services.Repositories.Abstractions;
using System.Diagnostics;

namespace Infrastructure.Repositories.Implementations
{
    public class UserRepository:GenericRepository<User>,IUserRepository
    {
        public UserRepository(DateBaseContext context) : base(context)
        {           
        }
    }
}
