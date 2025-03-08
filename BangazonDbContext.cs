using Microsoft.EntityFrameworkCore;
using Bangazon.Models;

public class BangazonDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Seller> Sellers { get; set; }
    public DbSet<Store> Stores { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<PaymentType> PaymentTypes { get; set; }
    public DbSet<Order> Orders { get; set; }

    public BangazonDbContext(DbContextOptions<BangazonDbContext> context) : base(context)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasData(new User[]
        {
            new User { Id = 1, Uid = "yZ123AbC456DeFgHijKlmN789Opq", FirstName = "Sirena", LastName = "Foster", Email = "sirena@example.com", Address = "123 Main St", City = "Nashville", Zip = 37209, IsSeller = true },
            new User { Id = 2, Uid = "aBcD987xYz654wVuTsQrMnOpL321Gh", FirstName = "Steven", LastName = "Robinson", Email = "steven@example.com", Address = "456 Elm St", City = "Knoxville", Zip = 37920, IsSeller = false }
        });

        modelBuilder.Entity<Customer>().HasData(new Customer[]
        {
            new Customer { Id = 1, UserId = "yZ123AbC456DeFgHijKlmN789Opq" },
            new Customer { Id = 2, UserId = "aBcD987xYz654wVuTsQrMnOpL321Gh" } // Steven Robinson
        });

        modelBuilder.Entity<Seller>().HasData(new Seller[]
        {
            new Seller { Id = 1, StoreId = 1, SellerId = "yZ123AbC456DeFgHijKlmN789Opq" } // Sirena Foster
        });

        modelBuilder.Entity<Store>().HasData(new Store[]
        {
            new Store { Id = 1, Name = "E-Lit", SellerId = "yZ123AbC456DeFgHijKlmN789Opq" } // Store owned by Sirena
        });

        modelBuilder.Entity<Category>().HasData(new Category[]
        {
            new Category { Id = 1, Title = "eReaders" },
            new Category { Id = 2, Title = "Accessories" }
        });

        modelBuilder.Entity<Product>().HasData(new Product[]
        {
            new Product { Id = 1, Name = "Color eReader", IsAvailable = true, Price = 349, Image = "eReader.jpg", Description = "Portable Color eReader", Quantity = 5, CategoryId = 1, StoreId = 1 },
            new Product { Id = 2, Name = "Stylus", IsAvailable = true, Price = 70, Image = "stylus.jpg", Description = "White Stylus", Quantity = 10, CategoryId = 2, StoreId = 1 }
        });

        modelBuilder.Entity<Cart>().HasData(new Cart[]
        {
            new Cart { Id = 1, ProductId = 1, TotalCost = 349, CustomerId = "aBcD987xYz654wVuTsQrMnOpL321Gh", PaymentType = 1 }
        });

        modelBuilder.Entity<PaymentType>().HasData(new PaymentType[]
        {
            new PaymentType { Id = 1, Type = "Credit Card", CustomerId = "aBcD987xYz654wVuTsQrMnOpL321Gh" },
            new PaymentType { Id = 2, Type = "PayPal", CustomerId = "yZ123AbC456DeFgHijKlmN789Opq" }
        });

        modelBuilder.Entity<Order>().HasData(new Order[]
        {
            new Order { Id = 1, CustomerId = "aBcD987xYz654wVuTsQrMnOpL321Gh", IsComplete = false, CartId = 1 }
        });
    }
}
