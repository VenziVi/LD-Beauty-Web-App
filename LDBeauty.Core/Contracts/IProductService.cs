using LDBeauty.Core.Models.Product;
using LDBeauty.Infrastructure.Data.Identity;

namespace LDBeauty.Core.Contracts
{
    public interface IProductService
    {
        Task AddProduct(AddProductViewModel model);
        Task<AllProductsViewModel> GetAllProducts();
        Task<ProductDetailsViewModel> GetProduct(int id);
        Task EditProduct(AddProductViewModel model, int id);
        Task AddToFavourites(int productId, ApplicationUser user);
        Task<List<GetProductViewModel>> GetFavouriteProducts(ApplicationUser user);
        Task RemoveFromFavourite(int id, ApplicationUser user);
        Task<AllProductsViewModel> GetProductsByCategory(int id);
        Task<AllProductsViewModel> GetProductsByMake(int id);
        Task<AllProductsViewModel> GetProductsByName(string productName);
    }
}
