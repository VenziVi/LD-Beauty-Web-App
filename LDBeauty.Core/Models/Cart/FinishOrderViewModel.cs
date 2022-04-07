using LDBeauty.Core.Constants;
using System.ComponentModel.DataAnnotations;

namespace LDBeauty.Core.Models.Cart
{
    public class FinishOrderViewModel
    {
        [Required]
        [StringLength(50, ErrorMessage = ViewModelConstraints.MinMaxLengthError, MinimumLength = 3)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = ViewModelConstraints.MinMaxLengthError, MinimumLength = 3)]
        public string LastName { get; set; }

        [Required]
        [StringLength(200, ErrorMessage = ViewModelConstraints.MinMaxLengthError, MinimumLength = 10)]
        public string Address { get; set; }

        [Required]
        [RegularExpression(@"^\d{10}$", ErrorMessage = ViewModelConstraints.PhoneError)]
        public string Phone { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string CartId { get; set; }

        public string UserId { get; set; }

    }
}
