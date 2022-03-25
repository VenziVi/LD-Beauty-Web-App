using LDBeauty.Core.Models.Product;

namespace LDBeauty.Core.Contracts
{
    public interface IProductService
    {
        Task AddProduct(AddProductViewModel model);
        Task<IEnumerable<GetProductViewModel>> GetAllProducts();
        Task<ProductDetailsViewModel> GetProduct(string id);
    }
}
