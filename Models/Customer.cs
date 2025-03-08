using System.ComponentModel.DataAnnotations;

namespace Bangazon.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string UserId { get; set; } // Foreign key to User
    }
}
