using Domain.Entities;
using Infrastructure.EntityFramework;
using Services.Repositories.Abstractions;

namespace Infrastructure.Repositories.Implementations
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(DateBaseContext context) : base(context)
        {
        }
    }
}
