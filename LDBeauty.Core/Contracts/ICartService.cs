using LDBeauty.Core.Models.Cart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDBeauty.Core.Contracts
{
    public interface ICartService
    {
        Task AddToCart(AddToCartViewModel model);
    }
}
