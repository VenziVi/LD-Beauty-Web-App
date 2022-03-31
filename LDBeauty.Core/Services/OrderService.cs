using LDBeauty.Core.Contracts;
using LDBeauty.Core.Models.Cart;
using LDBeauty.Core.Models.User;
using LDBeauty.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDBeauty.Core.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext context;

        public OrderService(ApplicationDbContext _context)
        {
            context = _context;
        }

        public async Task FinishOrder(FinishOrderViewModel model)
        {
            Cart cart = await context.Set<Cart>()
                .FirstOrDefaultAsync(c => c.Id.ToString() == model.CartId);

            var productsList = await context.Set<AddedProduct>()
                .Where(a => a.CartId == cart.Id).ToListAsync();

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

                if (currProduct.Quantity < 0)
                {
                    throw new ArgumentException("You can't make an order becouse one or more products are out of stock!");
                }
            }

            foreach (var item in productsList)
            {
                item.Order = order;
                item.OrderId = order.Id;
            }

            context.Add(order);
            await context.SaveChangesAsync();
        }

        public async Task<List<UserProductsViewModel>> GetUserProducts(string userId)
        {
            var orders = await context.Set<Order>()
                .Where(o => o.ApplicationUserId == userId).ToListAsync();

            var userProducts = new List<UserProductsViewModel>();

            foreach (var order in orders)
            {
                var addedProducts = context.Set<AddedProduct>()
                    .Where(p => p.OrderId == order.Id)
                    .Include(p => p.Product)
                    .ThenInclude(m => m.Make)
                    .ToList();

                foreach (var item in addedProducts)
                {
                    userProducts.Add(new UserProductsViewModel()
                    {
                        Make = item.Product.Make.MakeName,
                        Name = item.Product.ProductName,
                        Date = item.Order.OrderDate.ToString("dd.MM.yyyy"),
                        Quantity = item.Quantity,
                        Price = item.Product.Price * item.Quantity
                    });
                }
            }

            return userProducts;
        }
    }
}
