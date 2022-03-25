using LDBeauty.Core.Contracts;
using LDBeauty.Core.Models.Product;
using LDBeauty.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDBeauty.Core.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext context;

        public ProductService(ApplicationDbContext _context)
        {
            context = _context;
        }

        public async Task AddProduct(AddProductViewModel model)
        {
            Category category = await context.Set<Category>()
                .FirstOrDefaultAsync(c => c.CategoryName == model.Category);

            if (category == null)
            {
                category = new Category()
                {
                    CategoryName = model.Category
                };

                context.Add(category);
            }

            Make make = await context.Set<Make>()
                .FirstOrDefaultAsync(c => c.MakeName == model.Make);

            if (make == null)
            {
                make = new Make()
                {
                    MakeName = model.Make
                };

                context.Add(make);
            }

            Product product = new Product()
            {
                ProductName = model.ProductName,
                ProductUrl = model.ProductUrl,
                Price = model.Price,
                Description = model.Description,
                Quantity = model.Quantity,
                Category = category,
                CategoryId = category.Id,
                Make = make,
                MakeId = make.Id
            };

            make.Products.Add(product);
            category.Products.Add(product);

            context.Add(product);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<GetProductViewModel>> GetAllProducts()
        {
            return await context.Set<Product>()
                .Select(p => new GetProductViewModel()
                {
                    Id = p.Id,
                    ProductName = p.ProductName,
                    ProductUrl = p.ProductUrl,
                    Price = p.Price,
                    Quantity = p.Quantity,
                    Make = p.Make.MakeName,
                }).ToListAsync();
        }

        public async Task<ProductDetailsViewModel> GetProduct(string id)
        {
            return await context.Set<Product>()
                .Where(p => p.Id.ToString() == id)
                .Select(p => new ProductDetailsViewModel()
                {
                    Id = p.Id,
                    ProductName = p.ProductName,
                    Description = p.Description,
                    ProductUrl = p.ProductUrl,
                    Price = p.Price,
                    Quantity = p.Quantity,
                    Make = p.Make.MakeName,
                    Category = p.Category.CategoryName
                }).FirstOrDefaultAsync();
        }
    }
}
