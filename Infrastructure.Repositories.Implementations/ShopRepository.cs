using Domain.Entities;
using Infrastructure.EntityFramework;
using Services.Repositories.Abstractions;

namespace Infrastructure.Repositories.Implementations
{
    public class ShopRepository : GenericRepository<Shop>, IShopRepository
    {
        public ShopRepository(DateBaseContext context) : base(context)
        {
        }
    }
}
