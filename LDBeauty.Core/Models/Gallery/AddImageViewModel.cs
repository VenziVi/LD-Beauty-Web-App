using LDBeauty.Core.Constraints;
using System.ComponentModel.DataAnnotations;

namespace LDBeauty.Core.Models
{
    public class AddImageViewModel
    {
        [Required]
        public string PictureUrl { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = ViewModelConstraints.MinMaxLengthError, MinimumLength = 5)]
        public string Category { get; set; }

        [Required]
        public string Description { get; set; }
    }
}
