using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDBeauty.Infrastructure.Data
{
    public class Client
    {
        public Guid Id { get; set; } = Guid.NewGuid();  

        public string Username { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public IList<Product> Favourites { get; set; } = new List<Product>();

        public IList<Order> Orders { get; set; } = new List<Order>();
    }
}
