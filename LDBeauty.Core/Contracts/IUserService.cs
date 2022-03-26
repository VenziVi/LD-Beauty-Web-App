using LDBeauty.Core.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDBeauty.Core.Contracts
{
    public interface IUserService
    {
        Task<UserOrderViewModel> GetUSerByName(string userName, string cartId);
    }
}
