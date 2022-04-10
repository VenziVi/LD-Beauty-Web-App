using LDBeauty.Core.Models.User;
using LDBeauty.Infrastructure.Data.Identity;

namespace LDBeauty.Core.Contracts
{
    public interface IUserService
    {
        Task<UserOrderViewModel> GetUSerByName(string userName, int cartId);
        Task<ApplicationUser> GetUser(string user);
        Task<List<AllUsersViewModel>> GetAllUsers(string name);
        Task<ApplicationUser> GetUserById(string id);
    }
}
