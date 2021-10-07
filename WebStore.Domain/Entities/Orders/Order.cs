using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebStore.Domain.Entities.Base;
using WebStore.Domain.Entities.Identity;

namespace WebStore.Domain.Entities.Orders
{
    public class Order : Entity
    {
        [Required]
        public User User { get; set; }

        [Required]
        [MaxLength(200)]
        public string Phone { get; set; }

        [Required]
        [MaxLength(500)]
        public string Address { get; set; }

        public string Description { get; set; }

        public DateTimeOffset Date { get; set; } = DateTimeOffset.UtcNow;

        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    }

    public class OrderItem : Entity
    {
        [Required]
        public Product Product { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public Order Order { get; set; }

        [NotMapped]
        public decimal TotalItemPrice => Price * Quantity;
    }
}
