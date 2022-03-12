using System.ComponentModel.DataAnnotations.Schema;

namespace LDBeauty.Infrastructure.Data
{
    public class ClientImage
    {
        [ForeignKey(nameof(Client))]
        public Guid ClientId { get; set; }

        public Client Client { get; set; }

        [ForeignKey(nameof(Image))]
        public int ImageId { get; set; }

        public Image Image { get; set; }
    }
}
