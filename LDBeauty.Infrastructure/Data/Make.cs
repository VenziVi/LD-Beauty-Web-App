using System.ComponentModel.DataAnnotations;

namespace LDBeauty.Infrastructure.Data
{
    public class Make
    {
        [Key]
        public int Id { get; init; }

        [Required]
        [StringLength(20)]
        public string MakeName { get; set; }

        public IList<Product> Products { get; set; } = new List<Product>();
    }
}
