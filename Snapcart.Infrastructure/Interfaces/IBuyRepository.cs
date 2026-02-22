using System;
using System.Collections.Generic;
using System.Text;
using Snapcart.Domain.Entities;

namespace Snapcart.Infrastructure.Interfaces
{
    public interface IBuyRepository
    {
        Task<IEnumerable<Buy>> GetBuysByUserIdAsync(Guid Id);
        Task<Buy> GetBuyByIdAsync(Guid Id);
        Task<Buy> AddBuyAsync(Buy buy);
        Task<Buy> UpdateBuyAsync(Buy buy);
        Task DeleteBuyAsync(Guid Id);
    }
}
