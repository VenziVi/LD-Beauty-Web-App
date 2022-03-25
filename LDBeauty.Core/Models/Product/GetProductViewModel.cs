using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDBeauty.Core.Models.Product
{
    public class GetProductViewModel
    {
        public Guid Id { get; set; }

        public string ProductName { get; set; }

        public string ProductUrl { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public string Make { get; set; }
    }
}
