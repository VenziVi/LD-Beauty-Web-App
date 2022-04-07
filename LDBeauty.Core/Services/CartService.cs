using LDBeauty.Core.Constants;
using LDBeauty.Core.Contracts;
using LDBeauty.Core.Models.Cart;
using LDBeauty.Infrastructure.Data;
using LDBeauty.Infrastructure.Data.Identity;
using LDBeauty.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LDBeauty.Core.Services
{
    public class CartService : ICartService
    {
        private readonly IApplicationDbRepository repo;

        public CartService(IApplicationDbRepository _repo)
        {
            repo = _repo;
        }

        public async Task AddToCart(AddToCartViewModel model, string userName)
        {
            var user = await GetUserByUserName(userName);

            Cart cart = await repo.All<Cart>()
                .FirstOrDefaultAsync(c => c.IsDeleted == false && c.UserId == user.Id);

            if (cart == null)
            {
                cart = new Cart()
                {
                    TotalPrice = 0.0M,
                    User = user,
                    UserId = user.Id
                };

                await repo.AddAsync(cart);
            }

            Product product = await repo.All<Product>()
                .FirstOrDefaultAsync(p => p.Id == model.ProductId);

            if (product == null)
            {
                throw new ArgumentException(ErrorMessages.ProductNotFound);
            }

            AddedProduct addedProduct = new AddedProduct()
            {
                ProductId = product.Id,
                Product = product,
                Quantity = model.Quantity,
                Cart = cart,
                CartId = cart.Id
            };

            cart.AddedProducts.Add(addedProduct);

            decimal price = 0.0M;

            foreach (var item in cart.AddedProducts)
            {
                price += item.Quantity * item.Product.Price;
            }

            cart.TotalPrice += price;

            await repo.AddAsync(addedProduct);

            await repo.SaveChangesAsync();

        }

        public async Task DeleteProduct(string id, string userName)
        {
            var user = await GetUserByUserName(userName);

            var userId = user.Id;

            var cart = await repo.All<Cart>()
                .FirstOrDefaultAsync(c => c.UserId == userId && c.IsDeleted == false);
                
            var cartId = cart.Id;

            AddedProduct product = await repo.All<AddedProduct>()
                .Where(p => p.ProductId.ToString() == id && p.CartId == cartId)
                .FirstOrDefaultAsync();

            Product currProduct = await repo.All<Product>()
                .FirstOrDefaultAsync(p => p.Id == product.ProductId);

            cart.AddedProducts.Remove(product);

            var qty = product.Quantity;
            var price = currProduct.Price;

            cart.TotalPrice -= qty * price;

            repo.Delete<AddedProduct>(product);

            if (cart.TotalPrice == 0)
            {
                repo.Delete<Cart>(cart);
            }

            await repo.SaveChangesAsync();
        }

        public async Task<CartDetailsViewModel> GetCart(string userName)
        {
            var user = await GetUserByUserName(userName);

            var cart = await repo.All<Cart>()
                .FirstOrDefaultAsync(c => c.IsDeleted == false && c.UserId == user.Id);

            if (cart == null)
            {
                return null;
            }

            var productsList = await repo.All<AddedProduct>()
                .Where(a => a.CartId == cart.Id).ToListAsync();

            var currOrder = new CartDetailsViewModel()
            {
                Id = cart.Id,
                TotalPrice = cart.TotalPrice,
            };

            currOrder.Products = await repo.All<AddedProduct>()
                .Where(a => a.CartId == cart.Id)
                .Select(p => new CartProductsViewModel()
                {
                    Id = p.ProductId.ToString(),
                    ProductName = p.Product.ProductName,
                    ProductMake = p.Product.Make.MakeName,
                    Quantity = p.Quantity,
                    Price = (p.Product.Price * p.Quantity).ToString("f2")
                }).ToListAsync();

            return currOrder;
        }

        private async Task<ApplicationUser> GetUserByUserName(string userName)
        {
            return await repo.All<ApplicationUser>()
                .SingleOrDefaultAsync(u => u.UserName == userName);
        }
    }
}
