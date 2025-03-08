using System.ComponentModel.DataAnnotations.Schema;

namespace Bangazon.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public int ProductId { get; set; } // Foreign key to Product
        public int TotalCost { get; set; }
        public string CustomerId { get; set; } // Foreign key to Customer
        public int PaymentType { get; set; } // Foreign key to PaymentType

        [ForeignKey("ProductId")]
        public Product Product { get; set; }
    }
}
