using LDBeauty.Core.Contracts;
using LDBeauty.Core.Models.Cart;
using LDBeauty.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDBeauty.Core.Services
{
    public class CartService : ICartService
    {
        private readonly ApplicationDbContext context;

        public CartService(ApplicationDbContext _context)
        {
            context = _context;
        }

        public async Task AddToCart(AddToCartViewModel model)
        {
            Cart cart = await context.Set<Cart>()
                .FirstOrDefaultAsync();

            if (cart == null)
            {
                cart = new Cart()
                {
                    TotalPrice = 0.0M
                };

                context.Add(cart);
            }

            Product product = await context.Set<Product>()
                .FirstOrDefaultAsync(p => p.Id == model.ProductId);

            AddedProduct addedProduct = new AddedProduct()
            {
                ProductId = product.Id,
                Product = product,
                Quantity = model.Quantity,
                Cart = cart,
                CartId = cart.Id
            };

            context.Add(addedProduct);

            cart.AddedProducts.Add(addedProduct);

            decimal price = 0.0M;

            foreach (var item in cart.AddedProducts)
            {
                price += item.Quantity * item.Product.Price;
            }

            cart.TotalPrice = price;

            await context.SaveChangesAsync();

        }
    }
}
