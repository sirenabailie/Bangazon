using System.ComponentModel.DataAnnotations;

namespace Bangazon.Models
{
    public class PaymentType
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string CustomerId { get; set; } // Foreign key to Customer
    }
}
