using LDBeauty.Infrastructure.Data.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace LDBeauty.Infrastructure.Data
{
    public class UserImage
    {
        [ForeignKey(nameof(ApplicationUser))]
        public string ApplicationUserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        [ForeignKey(nameof(Image))]
        public int ImageId { get; set; }

        public Image Image { get; set; }
    }
}
