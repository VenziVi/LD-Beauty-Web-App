namespace LDBeauty.Core.Models.Product
{
    public class GetProductViewModel
    {
        public Guid Id { get; set; }

        public string ProductName { get; set; }

        public string ProductUrl { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public string Make { get; set; }

        public int MakeId { get; set; }

        public string Category { get; set; }

        public int CategoryId { get; set; }
    }
}
