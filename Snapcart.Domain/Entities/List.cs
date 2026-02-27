using System;
using System.Collections.Generic;
using System.Text;

namespace Snapcart.Domain.Entities
{
    public class List
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; } 
        public User User { get; set; }
        public bool IsActive { get; set; } = true;
        public List<Product> Products { get; set; } = new List<Product>();
        public int? TotalPrice { get; set; }
    }
}
