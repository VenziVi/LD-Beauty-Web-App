using LDBeauty.Core.Contracts;
using LDBeauty.Core.Models.Product;
using LDBeauty.Infrastructure.Data;
using LDBeauty.Infrastructure.Data.Identity;
using Microsoft.AspNet.Identity;
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
            Category category = await CreateCategory(model.Category);

            Make make = await CreateMake(model.Make);

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

            await context.AddAsync(product);
            await context.SaveChangesAsync();
        }

        public async Task AddToFavourites(string productId, ApplicationUser user)
        {
            Product product = await context.Set<Product>()
                .FirstOrDefaultAsync(p => p.Id.ToString() == productId);

            UserProduct userProduct = new UserProduct()
            {
                ProductId = product.Id,
                Product = product,
                ApplicationUserId = user.Id,
                ApplicationUser = user
            };

            user.FavouriteProducts.Add(product);

            await context.AddAsync(userProduct);
            await context.SaveChangesAsync();
        }

        public async Task EditProduct(AddProductViewModel model, string id)
        {
            Product product = await context.Set<Product>()
                .FirstOrDefaultAsync(p => p.Id.ToString() == id);

            Category category = await CreateCategory(model.Category);

            Make make = await CreateMake(model.Make);

            product.ProductName = model.ProductName;
            product.Description = model.Description;
            product.Quantity = model.Quantity;
            product.Price = model.Price;
            product.ProductUrl = model.ProductUrl;
            product.Category = category;
            product.CategoryId = category.Id;
            product.Make = make;
            product.MakeId = make.Id;

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

        public async Task<List<GetProductViewModel>> GetFavouriteProducts(ApplicationUser user)
        {
            List<GetProductViewModel> products = await context.Set<UserProduct>()
                .Where(p => p.ApplicationUser.Id == user.Id)
                .Select(p => new GetProductViewModel() 
                {
                    ProductName = p.Product.ProductName,
                    ProductUrl = p.Product.ProductUrl,
                    Price = p.Product.Price,
                    Id = p.ProductId,
                    Make = p.Product.Make.MakeName,
                    Quantity = p.Product.Quantity
                }).ToListAsync();

            return products;
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

        public async Task RemoveFromFavourite(string id, ApplicationUser user)
        {
            Product product = await context.Set<Product>()
                .FirstOrDefaultAsync(p => p.Id.ToString() == id);

            UserProduct userProduct = await context.Set<UserProduct>()
                .FirstOrDefaultAsync(p => p.ProductId.ToString() == id &&
                p.ApplicationUserId == user.Id);

            user.FavouriteProducts.Remove(product);
            context.Remove(userProduct);
            
            await context.SaveChangesAsync();
        }

        private async Task<Category> CreateCategory(string name)
        {
            Category category = await context.Set<Category>()
                .FirstOrDefaultAsync(c => c.CategoryName == name);

            if (category == null)
            {
                category = new Category()
                {
                    CategoryName = name
                };

                context.Add(category);
            }

            return category;
        }

        private async Task<Make> CreateMake(string name)
        {
            Make make = await context.Set<Make>()
                .FirstOrDefaultAsync(c => c.MakeName == name);

            if (make == null)
            {
                make = new Make()
                {
                    MakeName = name
                };

                context.Add(make);
            }

            return make;
        }
    }
}
