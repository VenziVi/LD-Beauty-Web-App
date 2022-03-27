using LDBeauty.Core.Contracts;
using LDBeauty.Core.Models.Cart;
using LDBeauty.Infrastructure.Data;
using LDBeauty.Infrastructure.Data.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
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

        public async Task AddToCart(AddToCartViewModel model, string userName)
        {
            var user = await context.Set<ApplicationUser>()
                .SingleOrDefaultAsync(u => u.UserName == userName);

            Cart cart = await context.Set<Cart>()
                .FirstOrDefaultAsync(c => c.IsDeleted == false);

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

            cart.AddedProducts.Add(addedProduct);

            decimal price = 0.0M;

            foreach (var item in cart.AddedProducts)
            {
                price += item.Quantity * item.Product.Price;
            }

            cart.TotalPrice += price;

            context.Add(addedProduct);


            await context.SaveChangesAsync();

        }

        public async Task DeleteProduct(string id)
        {
            var ids = id.Split(":");
            var productId = ids[0];
            var cartId = ids[1];

            Cart cart = await context.Set<Cart>()
                .FirstOrDefaultAsync(c => c.Id.ToString() == cartId);

            AddedProduct product = await context.Set<AddedProduct>()
                .Where(p => p.ProductId.ToString() == productId && p.CartId.ToString() == cartId)
                .FirstOrDefaultAsync();

            Product currProduct = await context.Set<Product>()
                .FirstOrDefaultAsync(p => p.Id == product.ProductId);

            cart.AddedProducts.Remove(product);

            var qty = product.Quantity;
            var price = currProduct.Price;

            cart.TotalPrice -= qty * price;

            context.Remove(product);

            if (cart.TotalPrice == 0)
            {
                context.Remove(cart);
            }

            context.SaveChangesAsync();
        }

        public async Task FinishOrder(FinishOrderViewModel model)
        {
            Cart cart = context.Set<Cart>()
                .FirstOrDefault(c => c.Id.ToString() == model.CartId);

            var productsList = context.Set<AddedProduct>()
                .Where(a => a.CartId == cart.Id).ToList();

            Order order = new Order()
            {
                ClientFirstName = model.FirstName,
                ClientLastName = model.LastName,
                Address = model.Address,
                Phone = model.Phone,
                Email = model.Email,
                OrderDate = DateTime.Now,
                TotalPrice = cart.TotalPrice,
                Products = productsList,
                ApplicationUserId = model.UserId
            };

            cart.IsDeleted = true;

            foreach (var product in productsList)
            {
                var quantity = product.Quantity;

                var currProduct = context.Set<Product>()
                    .FirstOrDefault(p => p.Id == product.ProductId);

                currProduct.Quantity -= quantity;
            }

            context.Add(order);
            await context.SaveChangesAsync();
        }

        public async Task<CartDetailsViewModel> GetCart()
        {
            var cart = context.Set<Cart>()
                .FirstOrDefault(c => c.IsDeleted == false);

            if (cart == null)
            {
                return null;
            }

            var productsList = context.Set<AddedProduct>()
                .Where(a => a.CartId == cart.Id).ToList();

            var currOrder = new CartDetailsViewModel()
            {
                Id = cart.Id,
                TotalPrice = cart.TotalPrice,
            };

            currOrder.Products = context.Set<AddedProduct>()
                .Where(a => a.CartId == cart.Id)
                .Select(p => new CartProductsViewModel()
                {
                    Id = p.ProductId.ToString(),
                    ProductName = p.Product.ProductName,
                    ProductMake = p.Product.Make.MakeName,
                    Quantity = p.Quantity,
                    Price = (p.Product.Price * p.Quantity).ToString("f2")
                }).ToList();

            return currOrder;
        }
    }
}
