using LDBeauty.Core.Models.Product;

namespace LDBeauty.Core.Contracts
{
    public interface IProductService
    {
        Task AddProduct(AddProductViewModel model);
    }
}
