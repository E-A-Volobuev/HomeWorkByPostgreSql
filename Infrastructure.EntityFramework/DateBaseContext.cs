using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EntityFramework
{
    public class DateBaseContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Shop> Shops { get; set; }
        public DateBaseContext(DbContextOptions<DateBaseContext> options) : base(options)
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //как таковой надобности в этом коде нет,я тестировал возможности FluentApi
            modelBuilder.Entity<User>().ToTable("People");
            modelBuilder.Entity<Product>().Property(x => x.Id).HasColumnName("product_id");

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Shop)
                .WithMany(t => t.Products)
                .HasForeignKey(p => p.ShopId);

            modelBuilder.Entity<Shop>()
               .HasOne(p => p.User)
               .WithMany(t => t.Shops)
               .HasForeignKey(p => p.UserId);

            modelBuilder.Entity<User>().Property(u => u.Age).HasDefaultValue(18);
            modelBuilder.Entity<Product>().Property(u => u.Price).HasDefaultValue(1);

            //а вот тут идёт инициализация бд начальными значаниями
            modelBuilder.Entity<User>().HasData(GetUserArrayHelper());
            modelBuilder.Entity<Shop>().HasData(GetShopArrayHelper());
            modelBuilder.Entity<Product>().HasData(GetProductArrayHelper());
        }

        #region db initialize helpers
        private User[] GetUserArrayHelper()
        {
            var users = new User[]
            {
                new User { Id=1, Name="Tom", Age=23},
                new User { Id=2, Name="Alice"},
                new User { Id=3, Name="Bill"},
                new User { Id=4, Name="John", Age=28},
                new User { Id=5, Name="Sam", Age=30}
            };

            return users;
        }

        private Shop[] GetShopArrayHelper()
        {
            var users = new Shop[]
            {
                new Shop { Id=1, Name="TomShop", UserId=1},
                new Shop { Id=2, Name="AliceShop", UserId=2},
                new Shop { Id=3, Name="BillShop", UserId=3},
                new Shop { Id=4, Name="JohnShop", UserId=4},
                new Shop { Id=5, Name="SamShop", UserId=5}
            };

            return users;
        }
        private Product[] GetProductArrayHelper()
        {
            var users = new Product[]
            {
                new Product { Id=1, Name="TomShopProduct",Price=100, ShopId=1},
                new Product { Id=2, Name="AliceShopProduct", ShopId=2},
                new Product { Id=3, Name="BillShopProduct", ShopId=3},
                new Product { Id=4, Name="JohnShopProduct",Price=400, ShopId=4},
                new Product { Id=5, Name="SamShopProduct", Price=500,ShopId=5}
            };

            return users;
        }
        #endregion
    }
}
