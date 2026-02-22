using System;
using System.Collections.Generic;
using System.Text;

namespace Snapcart.Domain.Entities
{
    /// <summary>
    /// Entidad que representa un item de la lista de compras
    /// </summary>
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Brand { get; set; }
        public int? Quantity { get; set; }
        public int? Price { get; set; }
        public string? Category { get; set; }

        public Guid ListId { get; set; }
        public List List { get; set; }
    }
}
