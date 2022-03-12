using System.ComponentModel.DataAnnotations;

namespace LDBeauty.Core.Models
{
    public class AddImageViewModel
    {
        [Required]
        public string PictureUrl { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 5)]
        public string Category { get; set; }

        [Required]
        public string Description { get; set; }
    }
}
