using LDBeauty.Infrastructure.Data.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LDBeauty.Infrastructure.Data
{
    public class Cart
    {
        [Key]
        public int Id { get; init; }

        public IList<AddedProduct> AddedProducts { get; set; } = new List<AddedProduct>();

        public decimal TotalPrice { get; set; }

        public bool IsDeleted { get; set; } = false;

        [ForeignKey(nameof(User))]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }
    }
}
