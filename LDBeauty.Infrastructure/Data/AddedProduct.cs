using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LDBeauty.Infrastructure.Data
{
    public class AddedProduct
    {
        [Key]
        public int Id { get; init; }

        public int ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; }

        [Required]
        public int Quantity { get; set; }

        public int CartId { get; set; }

        [ForeignKey(nameof(CartId))]
        public Cart Cart { get; set; }

        [ForeignKey(nameof(Order))]
        public int? OrderId { get; set; }

        public Order? Order { get; set; }

    }
}
