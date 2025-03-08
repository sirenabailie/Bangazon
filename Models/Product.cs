using System.ComponentModel.DataAnnotations;

namespace Bangazon.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsAvailable { get; set; }
        public int Price { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public int CategoryId { get; set; } // Foreign key to Category
        public int StoreId { get; set; } // Foreign key to Store
    }
}
