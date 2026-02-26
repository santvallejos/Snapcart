using System;
using System.Collections.Generic;
using System.Text;
using Snapcart.Domain.Entities;

namespace Snapcart.Application.Dtos
{
    public class ListDto
    {
        public Guid UserId { get; set; }
        public List<Product> Products { get; set; } = new List<Product>();
        public int? TotalPrice { get; set; }
    }
}
