using System.ComponentModel.DataAnnotations;

namespace LDBeauty.Infrastructure.Data
{
    public class ImgCategory
    {
        [Key]
        public int Id { get; init; }

        [Required]
        [StringLength(20)]
        public string CategoryName { get; set; }

        [Required]
        public string ImgUrl { get; set; }

        public IList<Image> Images { get; set; } = new List<Image>();
    }
}
