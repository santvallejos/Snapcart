using System;
using System.Collections.Generic;
using System.Text;
using Snapcart.Domain.Entities;

namespace Snapcart.Infrastructure.Interfaces
{
    public interface IListRepository
    {
        Task<IEnumerable<List>> GetAllListsByUserIdAsync(Guid userId);
        Task<List?> GetActiveListByUserIdAsync(Guid userId);
        Task<List> GetListByIdAsync(Guid Id);
        Task<List> AddListAsync(List list);
        Task<List> UpdateListAsync(List list);
        Task DeleteListAsync(Guid id);
    }
}
