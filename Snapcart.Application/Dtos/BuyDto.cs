using System;
using System.Collections.Generic;
using System.Text;

namespace Snapcart.Application.Dtos
{
    public class BuyDto
    {
        public DateTime PurchaseCompletedAt { get; set; }
        public string? SupermarketName { get; set; }
    }
}
