using System.ComponentModel.DataAnnotations;

namespace Bangazon.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Uid { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public int Zip { get; set; }
        public bool IsSeller { get; set; }
    }
}
