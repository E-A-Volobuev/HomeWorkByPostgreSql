
namespace Domain.Entities
{
    public class Product: IEntity
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }

        public int ShopId { get; set; }
        public Shop? Shop { get; set; }
    }
}
