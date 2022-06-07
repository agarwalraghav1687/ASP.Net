using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Play.Catalog.Serice.Entities;

namespace Play.Catalog.Serice.Repo
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