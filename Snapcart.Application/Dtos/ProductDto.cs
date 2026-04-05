using System;
using System.Collections.Generic;
using System.Text;

namespace Snapcart.Application.Dtos
{
    public class ProductDto
    {
        public string Name { get; set; }
        public string? Brand { get; set; }
        public int? Quantity { get; set; }
        public int? Price { get; set; }
        public string? Category { get; set; }
    }
}
