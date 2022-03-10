using System.ComponentModel.DataAnnotations;

namespace LDBeauty.Infrastructure.Data
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string CategoryName { get; set; }

        public IList<Product> Products { get; set; } = new List<Product>();
    }
}
