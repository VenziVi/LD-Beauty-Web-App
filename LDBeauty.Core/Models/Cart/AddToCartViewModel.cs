using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDBeauty.Core.Models.Cart
{
    public class AddToCartViewModel
    {
        public Guid ProductId { get; set; }

        public int Quantity { get; set; }
    }
}
