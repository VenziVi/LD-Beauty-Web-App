﻿using LDBeauty.Core.Models.User;
using LDBeauty.Infrastructure.Data.Identity;
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
        Task<ApplicationUser> GetUser(string user);
    }
}
