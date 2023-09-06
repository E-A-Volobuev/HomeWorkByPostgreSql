
namespace Domain.Entities
{
    public class User : IEntity
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Age { get; set; }
        public List<Shop>? Shops { get; set; }
    }
}
