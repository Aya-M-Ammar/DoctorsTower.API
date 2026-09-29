using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace DoctorsTower.infrastructure.Contract
{
   
        public interface IGenericRepository<TEntity> where TEntity : class
        {
        public Task AddAsync(TEntity entity);

        public void Delete(TEntity entity);


        public Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate);

        public Task<TEntity?> GetByIdAsync(int id);

        public void Update(TEntity entity);
    }
    
}
