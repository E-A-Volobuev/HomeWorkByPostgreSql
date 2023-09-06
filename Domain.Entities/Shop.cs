
namespace Domain.Entities
{
    public class Shop : IEntity
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public List<Product>? Products { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }
    }
}
