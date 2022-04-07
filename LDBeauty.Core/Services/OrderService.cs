using LDBeauty.Core.Constants;
using LDBeauty.Core.Contracts;
using LDBeauty.Core.Models.Cart;
using LDBeauty.Core.Models.User;
using LDBeauty.Infrastructure.Data;
using LDBeauty.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LDBeauty.Core.Services
{
    public class OrderService : IOrderService
    {
        private readonly IApplicationDbRepository repo;

        public OrderService(IApplicationDbRepository _repo)
        {
            repo = _repo;
        }

        public async Task FinishOrder(FinishOrderViewModel model)
        {
            Cart cart = await repo.All<Cart>()
                .FirstOrDefaultAsync(c => c.Id.ToString() == model.CartId);

            var productsList = await repo.All<AddedProduct>()
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

                var currProduct = repo.All<Product>()
                    .FirstOrDefault(p => p.Id == product.ProductId);

                currProduct.Quantity -= quantity;

                if (currProduct.Quantity < 0)
                {
                    throw new ArgumentException(ErrorMessages.OutOfStockError);
                }
            }

            foreach (var item in productsList)
            {
                item.Order = order;
                item.OrderId = order.Id;
            }

            await repo.AddAsync(order);
            await repo.SaveChangesAsync();
        }

        public async Task<List<UserProductsViewModel>> GetUserProducts(string userId)
        {
            var orders = await repo.All<Order>()
                .Where(o => o.ApplicationUserId == userId).ToListAsync();

            var userProducts = new List<UserProductsViewModel>();

            foreach (var order in orders.OrderByDescending(i => i.OrderDate))
            {
                var addedProducts = await repo.All<AddedProduct>()
                    .Where(p => p.OrderId == order.Id)
                    .Include(p => p.Product)
                    .ThenInclude(m => m.Make)
                    .ToListAsync();

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
