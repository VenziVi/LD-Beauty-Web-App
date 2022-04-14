using LDBeauty.Core.Constants;
using LDBeauty.Core.Contracts;
using LDBeauty.Core.Models.Product;
using LDBeauty.Infrastructure.Data;
using LDBeauty.Infrastructure.Data.Identity;
using LDBeauty.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LDBeauty.Core.Services
{
    public class ProductService : IProductService
    {
        private readonly IApplicationDbRepository repo;

        public ProductService(IApplicationDbRepository _repo)
        {
            repo = _repo;
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

            await repo.AddAsync(product);
            await repo.SaveChangesAsync();
        }

        public async Task AddToFavourites(int productId, ApplicationUser user)
        {
            Product product = await repo.All<Product>()
                .FirstOrDefaultAsync(p => p.Id == productId);

            if (product == null)
            {
                throw new ArgumentException(ErrorMessages.ProductNotFound);
            }

            UserProduct userProduct = new UserProduct()
            {
                ProductId = product.Id,
                Product = product,
                ApplicationUserId = user.Id,
                ApplicationUser = user
            };

            user.FavouriteProducts.Add(product);

            await repo.AddAsync(userProduct);
            await repo.SaveChangesAsync();
        }

        public async Task EditProduct(AddProductViewModel model, int id)
        {
            Product product = await repo.All<Product>()
                .FirstOrDefaultAsync(p => p.Id == id);

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

            await repo.SaveChangesAsync();
        }

        public async Task<AllProductsViewModel> GetAllProducts()
        {
            var products = await repo.AllReadonly<Product>()
                .Select(p => new GetProductViewModel()
                {
                    Id = p.Id,
                    ProductName = p.ProductName,
                    ProductUrl = p.ProductUrl,
                    Price = p.Price,
                    Quantity = p.Quantity,
                    Make = p.Make.MakeName
                }).ToListAsync();

            return await GetResult(products);
        }

        public async Task<List<GetProductViewModel>> GetFavouriteProducts(ApplicationUser user)
        {
            List<GetProductViewModel> products = await repo.All<UserProduct>()
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

        public async Task<ProductDetailsViewModel> GetProduct(int id)
        {
            return await repo.All<Product>()
                .Where(p => p.Id == id)
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

        public async Task<AllProductsViewModel> GetProductsByCategory(int id)
        {
            var products = await repo.AllReadonly<Product>()
                .Where(p => p.CategoryId == id)
                .Select(p => new GetProductViewModel()
                {
                    Id = p.Id,
                    ProductName = p.ProductName,
                    ProductUrl = p.ProductUrl,
                    Price = p.Price,
                    Quantity = p.Quantity,
                    Make = p.Make.MakeName
                }).ToListAsync();

            return await GetResult(products);
        }

        public async Task<AllProductsViewModel> GetProductsByMake(int id)
        {
            var products = await repo.AllReadonly<Product>()
                .Where(p => p.MakeId == id)
                .Select(p => new GetProductViewModel()
                {
                    Id = p.Id,
                    ProductName = p.ProductName,
                    ProductUrl = p.ProductUrl,
                    Price = p.Price,
                    Quantity = p.Quantity,
                    Make = p.Make.MakeName
                }).ToListAsync();

            return await GetResult(products);
        }

        public async Task<AllProductsViewModel> GetProductsByName(string productName)
        {
            var products = await repo.AllReadonly<Product>()
                .Where(p => p.ProductName.Contains(productName))
                .Select(p => new GetProductViewModel()
                {
                    Id = p.Id,
                    ProductName = p.ProductName,
                    ProductUrl = p.ProductUrl,
                    Price = p.Price,
                    Quantity = p.Quantity,
                    Make = p.Make.MakeName
                }).ToListAsync();

            return await GetResult(products);
        }

        public async Task RemoveFromFavourite(int id, ApplicationUser user)
        {
            Product product = await repo.All<Product>()
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                throw new ArgumentException(ErrorMessages.ProductNotFound);
            }

            UserProduct userProduct = await repo.All<UserProduct>()
                .FirstOrDefaultAsync(p => p.ProductId == id &&
                p.ApplicationUserId == user.Id);

            user.FavouriteProducts.Remove(product);
            repo.Delete<UserProduct>(userProduct);
            
            await repo.SaveChangesAsync();
        }

        private async Task<Category> CreateCategory(string name)
        {
            Category category = await repo.All<Category>()
                .FirstOrDefaultAsync(c => c.CategoryName == name);

            if (category == null)
            {
                category = new Category()
                {
                    CategoryName = name
                };

                await repo.AddAsync(category);
            }

            return category;
        }

        private async Task<Make> CreateMake(string name)
        {
            Make make = await repo.All<Make>()
                .FirstOrDefaultAsync(c => c.MakeName == name);

            if (make == null)
            {
                make = new Make()
                {
                    MakeName = name
                };

                await repo.AddAsync(make);
            }

            return make;
        }

        private async Task<AllProductsViewModel> GetResult(List<GetProductViewModel> products)
        {
            return new AllProductsViewModel()
            {
                Products = products,
                Categories = await repo.AllReadonly<Category>()
                .ToListAsync(),
                Makes = await repo.AllReadonly<Make>()
                .ToListAsync()
            };
        }
    }
}
