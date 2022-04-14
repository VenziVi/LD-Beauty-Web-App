using LDBeauty.Core.Models.Product;
using LDBeauty.Infrastructure.Data.Identity;

namespace LDBeauty.Core.Contracts
{
    public interface IProductService
    {
        Task AddProduct(AddProductViewModel model);
        Task<List<GetProductViewModel>> GetAllProducts();
        Task<ProductDetailsViewModel> GetProduct(int id);
        Task EditProduct(AddProductViewModel model, int id);
        Task AddToFavourites(int productId, ApplicationUser user);
        Task<List<GetProductViewModel>> GetFavouriteProducts(ApplicationUser user);
        Task RemoveFromFavourite(int id, ApplicationUser user);
        Task<List<GetProductViewModel>> GetProductsByCategory(int id);
        Task<List<GetProductViewModel>> GetProductsByMake(int id);
        Task<List<GetProductViewModel>> GetProductsByName(string productName);
    }
}
