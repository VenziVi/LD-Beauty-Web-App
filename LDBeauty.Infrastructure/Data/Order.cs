﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LDBeauty.Infrastructure.Data
{
    public class Order
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid? ClientId { get; set; }

        [ForeignKey(nameof(ClientId))]
        public Client? Client { get; set; }

        public IList<AddedProduct> Products { get; set; } = new List<AddedProduct>();
    }
}
