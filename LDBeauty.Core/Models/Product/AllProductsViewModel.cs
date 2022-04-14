using LDBeauty.Infrastructure.Data;

namespace LDBeauty.Core.Models.Product
{
    public class AllProductsViewModel
    {
        public List<Category> Categories { get; set; }

        public List<Make> Makes { get; set; }

        public List<GetProductViewModel> Products { get; set; }
    }
}
