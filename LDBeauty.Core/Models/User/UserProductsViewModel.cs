using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDBeauty.Core.Models.User
{
    public class UserProductsViewModel
    {
        public string Make { get; set; }

        public string  Name { get; set; }

        public int Quantity { get; set; }

        public string Date { get; set; }

        public decimal Price { get; set; }
    }
}
