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
            var user = GetUserByUserName(userName);

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

        public async Task DeleteProduct(string id)
        {
            var ids = id.Split(":");
            var productId = ids[0];
            var cartId = ids[1];

            Cart cart = repo.All<Cart>()
                .FirstOrDefault(c => c.Id.ToString() == cartId);

            AddedProduct product = repo.All<AddedProduct>()
                .Where(p => p.ProductId.ToString() == productId && p.CartId.ToString() == cartId)
                .FirstOrDefault();

            Product currProduct = repo.All<Product>()
                .FirstOrDefault(p => p.Id == product.ProductId);

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
            var user = GetUserByUserName(userName);

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

        private ApplicationUser GetUserByUserName(string userName)
        {
            return repo.All<ApplicationUser>()
                .SingleOrDefault(u => u.UserName == userName);
        }
    }
}
