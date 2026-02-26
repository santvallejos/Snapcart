using System;
using System.Collections.Generic;
using System.Text;

namespace Snapcart.Domain.Entities
{
    public class Buy
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ListId { get; set; }
        public DateTime PurchaseCompletedAt { get; set; }
        public string? SupermarketName { get; set; }

        public User User { get; set; }
        public List List { get; set; }
    }
}
