using LDBeauty.Core.Contracts;
using LDBeauty.Core.Models.User;
using LDBeauty.Infrastructure.Data;
using LDBeauty.Infrastructure.Data.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDBeauty.Core.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext context;

        public UserService(ApplicationDbContext _context)
        {
            context = _context;
        }

        public async Task<List<AllUsersViewModel>> GetAllUsers()
        {
            return await context.Set<ApplicationUser>()
                .Select(x => new AllUsersViewModel()
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.Email,
                    Phone = context.Set<Order>()
                    .FirstOrDefault(o => o.ApplicationUserId == x.Id)
                    .Phone
                })
                .OrderBy(u => u.FirstName)
                .ThenBy(u => u.LastName)
                .ToListAsync();
        }

        public async Task<ApplicationUser> GetUser(string user)
        {
            return await context.Set<ApplicationUser>()
                .SingleOrDefaultAsync(u => u.UserName == user);

        }

        public async Task<ApplicationUser> GetUserById(string id)
        {
            return await context.Set<ApplicationUser>()
                .SingleOrDefaultAsync(u => u.Id == id);
        }

        public async Task<UserOrderViewModel> GetUSerByName(string userName, string cartId)
        {
            return await context.Set<ApplicationUser>()
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
