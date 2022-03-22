using System.ComponentModel.DataAnnotations;

namespace LDBeauty.Core.Models
{
    public class AddImageViewModel
    {
        [Required]
        public string PictureUrl { get; set; }

        [Required]
        [StringLength(20, ErrorMessage ="The {0} must be at least {2} and at max {1} characters long.",MinimumLength = 5)]
        public string Category { get; set; }

        [Required]
        public string Description { get; set; }
    }
}
