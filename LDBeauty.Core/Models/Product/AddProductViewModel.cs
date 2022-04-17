using LDBeauty.Core.Constants;
using System.ComponentModel.DataAnnotations;

namespace LDBeauty.Core.Models.Product
{
    public class AddProductViewModel
    {
        [Required]
        [StringLength(30, ErrorMessage = ViewModelConstraints.MinMaxLengthError, MinimumLength = 3)]
        public string ProductName { get; set; }

        [Required]
        [MinLength(10, ErrorMessage = ViewModelConstraints.MinLengthError)]
        public string ProductUrl { get; set; }

        [Required]
        [MinLength(10, ErrorMessage = ViewModelConstraints.MinLengthError)]
        public string Description { get; set; }

        [Range(1.00, 999.99, ErrorMessage = ViewModelConstraints.PriceError)]
        public decimal Price { get; set; }

        [Range(0, 100, ErrorMessage = ViewModelConstraints.QuantityError)]
        public int Quantity { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = ViewModelConstraints.MinMaxLengthError, MinimumLength = 3)]
        public string Category { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = ViewModelConstraints.MinMaxLengthError, MinimumLength = 3)]
        public string Make { get; set; }
    }
}
