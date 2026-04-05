using System;
using System.Collections.Generic;
using System.Text;

namespace Snapcart.Domain.Entities
{
    /// <summary>
    /// Entidad que representa un usuario en el sistema
    /// </summary>
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string HashPassword { get; set; }
        public string? Phone { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<List> Lists { get; set; } = new List<List>();
        public ICollection<Buy> Buys { get; set; } = new List<Buy>();
    }
}
