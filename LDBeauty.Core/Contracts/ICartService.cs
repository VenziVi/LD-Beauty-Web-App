using LDBeauty.Core.Models.Cart;

namespace LDBeauty.Core.Contracts
{
    public interface ICartService
    {
        Task AddToCart(AddToCartViewModel model, string userName);
        Task<CartDetailsViewModel> GetCart(string userName);       
        Task DeleteProduct(int id, string userName);
    }
}
