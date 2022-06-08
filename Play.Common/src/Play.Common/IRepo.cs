using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Play.Common
{
    public interface IRepo<T> where T : IEntity
    {
        Task CreateAsync(T entity);
        Task DeleteAsync(Guid id);
        Task<IReadOnlyCollection<T>> GetAllAsync();
        Task<T> GetAsync(Guid id);
        Task UpdateAync(T entity);
    }
}