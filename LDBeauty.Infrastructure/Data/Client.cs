using System.ComponentModel.DataAnnotations;

namespace LDBeauty.Infrastructure.Data
{
    public class Client
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();  

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [StringLength(200)]
        public string Address { get; set; }

        [Required]
        [StringLength(17)]
        [Phone]
        public string Phone { get; set; }

        public IList<Product> FavouriteProducts { get; set; } = new List<Product>();

        public IList<Image> FavouriteImages { get; set; } = new List<Image>();

        public IList<Order> Orders { get; set; } = new List<Order>();
    }
}
