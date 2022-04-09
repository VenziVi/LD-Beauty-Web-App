using LDBeauty.Core.Models.Cart;
using LDBeauty.Infrastructure.Data.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDBeauty.Core.Contracts
{
    public interface ICartService
    {
        Task AddToCart(AddToCartViewModel model, string userName);
        Task<CartDetailsViewModel> GetCart(string userName);       
        Task DeleteProduct(int id, string userName);
    }
}
