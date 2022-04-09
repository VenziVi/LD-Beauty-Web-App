using LDBeauty.Infrastructure.Data.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LDBeauty.Infrastructure.Data
{
    public class Image
    {
        [Key]
        public int Id { get; init; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        public int CategoruId { get; set; }

        [ForeignKey(nameof(CategoruId))]
        public ImgCategory Category { get; set; }
    }
}
