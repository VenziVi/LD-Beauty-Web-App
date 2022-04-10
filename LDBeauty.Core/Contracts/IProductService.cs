using LDBeauty.Core.Models.Product;
using LDBeauty.Infrastructure.Data.Identity;

namespace LDBeauty.Core.Contracts
{
    public interface IProductService
    {
        Task AddProduct(AddProductViewModel model);
        Task<IEnumerable<GetProductViewModel>> GetAllProducts();
        Task<ProductDetailsViewModel> GetProduct(int id);
        Task EditProduct(AddProductViewModel model, int id);
        Task AddToFavourites(int productId, ApplicationUser user);
        Task<List<GetProductViewModel>> GetFavouriteProducts(ApplicationUser user);
        Task RemoveFromFavourite(int id, ApplicationUser user);
        Task<IEnumerable<GetProductViewModel>> GetProductsByCategory(int id);
        Task<IEnumerable<GetProductViewModel>> GetProductsByMake(int id);
        Task<List<GetProductViewModel>> GetProductsByName(string productName);
    }
}
