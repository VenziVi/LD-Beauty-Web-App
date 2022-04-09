using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDBeauty.Core.Models.Cart
{
    public class CartDetailsViewModel
    {
        public int Id { get; set; }

        public decimal TotalPrice { get; set; }

        public List<CartProductsViewModel> Products { get; set; }
    }

    public class CartProductsViewModel
    {
        public string Id { get; set; }

        public string ProductName { get; set; }

        public string ProductMake { get; set; }

        public int Quantity { get; set; }

        public string Price { get; set; }
    }
}
