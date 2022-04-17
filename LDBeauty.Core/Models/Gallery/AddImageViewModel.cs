using LDBeauty.Core.Constants;
using System.ComponentModel.DataAnnotations;

namespace LDBeauty.Core.Models
{
    public class AddImageViewModel
    {
        [Required]
        [MinLength(10, ErrorMessage = ViewModelConstraints.MinLengthError)]
        public string PictureUrl { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = ViewModelConstraints.MinMaxLengthError, MinimumLength = 5)]
        public string Category { get; set; }

        [Required]
        [MinLength(10, ErrorMessage = ViewModelConstraints.MinLengthError)]
        public string Description { get; set; }
    }
}
