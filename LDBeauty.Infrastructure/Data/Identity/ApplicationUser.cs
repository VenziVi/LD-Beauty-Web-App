using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace LDBeauty.Infrastructure.Data.Identity
{
    public class ApplicationUser : IdentityUser
    { 

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        public IList<Product> FavouriteProducts { get; set; } = new List<Product>();

        public IList<Image> FavouriteImages { get; set; } = new List<Image>();

        public IList<Order> Orders { get; set; } = new List<Order>();
    }
}
