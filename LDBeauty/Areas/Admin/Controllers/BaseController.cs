using LDBeauty.Core.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LDBeauty.Areas.Admin.Controllers
{
    [Authorize(Roles = UserConstant.Roles.Administrator)]
    [Area("Admin")]
    public class BaseController : Controller
    {
    }
}
