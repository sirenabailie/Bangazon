using System.ComponentModel.DataAnnotations.Schema;

namespace Bangazon.Models
{
    public class Seller
    {
        public int Id { get; set; }
        public int StoreId { get; set; } // Foreign key to Store
        public string SellerId { get; set; } // Foreign key to User

        [ForeignKey("StoreId")]
        public Store Store { get; set; }
    }
}
