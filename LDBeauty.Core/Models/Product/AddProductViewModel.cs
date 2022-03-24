using System.ComponentModel.DataAnnotations;

namespace LDBeauty.Core.Models.Product
{
    public class AddProductViewModel
    {
        [Required]
        [StringLength(30, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        public string ProductName { get; set; }

        [Required]
        public string ProductUrl { get; set; }

        [Required]
        public string Description { get; set; }

        [Range(0.01, 999.99)]
        public decimal Price { get; set; }

        [Range(0, 100)]
        public int Quantity { get; set; }

        [Required]
        [StringLength(20, ErrorMessage ="The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        public string Category { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        public string Make { get; set; }
    }
}
