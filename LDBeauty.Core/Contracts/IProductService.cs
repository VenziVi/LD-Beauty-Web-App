using LDBeauty.Core.Models.Product;
using LDBeauty.Infrastructure.Data.Identity;

namespace LDBeauty.Core.Contracts
{
    public interface IProductService
    {
        Task AddProduct(AddProductViewModel model);
        Task<IEnumerable<GetProductViewModel>> GetAllProducts();
        Task<ProductDetailsViewModel> GetProduct(string id);
        Task EditProduct(AddProductViewModel model, string id);
        Task AddToFavourites(string productId, ApplicationUser user);
        Task<List<GetProductViewModel>> GetFavouriteProducts(ApplicationUser user);
        Task RemoveFromFavourite(string id, ApplicationUser user);
        Task<IEnumerable<GetProductViewModel>> GetProductsByCategory(int id);
        Task<IEnumerable<GetProductViewModel>> GetProductsByMake(int id);
    }
}
