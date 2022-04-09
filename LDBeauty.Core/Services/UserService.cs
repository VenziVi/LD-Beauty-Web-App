using LDBeauty.Core.Contracts;
using LDBeauty.Core.Models.User;
using LDBeauty.Infrastructure.Data;
using LDBeauty.Infrastructure.Data.Identity;
using LDBeauty.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LDBeauty.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IApplicationDbRepository repo;

        public UserService(IApplicationDbRepository _repo)
        {
            repo = _repo;
        }

        public async Task<List<AllUsersViewModel>> GetAllUsers()
        {
            return await repo.All<ApplicationUser>()
                .Select(x => new AllUsersViewModel()
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.Email,
                    Phone = repo.All<Order>()
                    .FirstOrDefault(o => o.ApplicationUserId == x.Id)
                    .Phone
                })
                .OrderBy(u => u.FirstName)
                .ThenBy(u => u.LastName)
                .ToListAsync();
        }

        public async Task<ApplicationUser> GetUser(string user)
        {
            return await repo.All<ApplicationUser>()
                .SingleOrDefaultAsync(u => u.UserName == user);

        }

        public async Task<ApplicationUser> GetUserById(string id)
        {
            return await repo.All<ApplicationUser>()
                .SingleOrDefaultAsync(u => u.Id == id);
        }

        public async Task<UserOrderViewModel> GetUSerByName(string userName, int cartId)
        {
            return await repo.All<ApplicationUser>()
                .Where(u => u.UserName == userName)
                .Select(u => new UserOrderViewModel()
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    Id = u.Id,
                    CartId = cartId
                }).FirstOrDefaultAsync();

        }
    }
}
