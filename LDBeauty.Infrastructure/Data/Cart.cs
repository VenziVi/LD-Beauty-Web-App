using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LDBeauty.Infrastructure.Data
{
    public class Cart
    {
        [Key]
        public Guid Id { get; set; }

        public IList<AddedProduct> AddedProducts { get; set; } = new List<AddedProduct>();

        public decimal TotalPrice { get; set; }
    }
}
