using System.ComponentModel.DataAnnotations.Schema;

namespace Bangazon.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string CustomerId { get; set; } // Foreign key to Customer
        public bool IsComplete { get; set; }
        public int CartId { get; set; } // Foreign key to Cart

        [ForeignKey("CartId")]
        public Cart Cart { get; set; }
    }
}
