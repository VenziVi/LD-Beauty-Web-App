using LDBeauty.Infrastructure.Data.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace LDBeauty.Infrastructure.Data
{
    public class UserProduct
    {
        public string ApplicationUserId { get; set; }

        [ForeignKey(nameof(ApplicationUserId))]
        public ApplicationUser ApplicationUser { get; set; }

        public int ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; }
    }
}
