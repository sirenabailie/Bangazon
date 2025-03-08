using System.ComponentModel.DataAnnotations;

namespace Bangazon.Models
{
    public class Store
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProductId { get; set; } // Foreign key to Products
        public string SellerId { get; set; } // Foreign key to Seller
    }
}
