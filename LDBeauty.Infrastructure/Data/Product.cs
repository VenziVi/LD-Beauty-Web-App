﻿using LDBeauty.Infrastructure.Data.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LDBeauty.Infrastructure.Data
{
    public class Product
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(30)]
        public string ProductName { get; set; }

        [Required]
        public string ProductUrl { get; set; }

        [Required]
        public string Description { get; set; }

        [Range(0.01, 999.99)]
        public decimal Price { get; set; }
        
        [Range(0, 100)]
        public int Quantity { get; set; }

        public int CategoryId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; }

        public int MakeId { get; set; }

        [ForeignKey(nameof(MakeId))]
        public Make Make { get; set; }

        public IList<ApplicationUser> UsersFavourite { get; set; } = new List<ApplicationUser>();
    }
}
