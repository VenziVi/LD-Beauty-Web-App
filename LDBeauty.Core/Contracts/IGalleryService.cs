using LDBeauty.Core.Models;
using LDBeauty.Core.Models.Gallery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDBeauty.Core.Contracts
{
    public interface IGalleryService
    {
        ErrorViewModel AddImage(AddImageViewModel model);
        Task<IEnumerable<GalleryCategoryViewModel>> GetMCategories();
    }
}
