using System.ComponentModel.DataAnnotations;

namespace LDBeauty.Infrastructure.Data
{
    public class Service
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string ServiceName { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string ImgUrl { get; set; }
    }
}
