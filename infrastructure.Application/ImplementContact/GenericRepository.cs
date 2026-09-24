using DoctorsTower.infrastructure.Contract;
using DoctorsTower.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace DoctorsTower.Application.ImplementContact
{
   
        public class GenericRepository<TEntity> : IGenericRepository<TEntity>
            where TEntity : class
        {
            private readonly DoctorsTowerDbContext _context;
          

            public GenericRepository(DoctorsTowerDbContext context)
            {
                _context = context;
              
            }

        public async Task AddAsync(TEntity entity) => await _context.Set<TEntity>().AddAsync(entity);



        public void Delete(TEntity entity) => _context.Set<TEntity>().Remove(entity);


        public async Task<IEnumerable<TEntity>> GetAllAsync() => await _context.Set<TEntity>().ToListAsync();

        public async Task<IEnumerable<TEntity>> GetAllAsync(
    Expression<Func<TEntity, bool>> predicate)
        {
            return await _context.Set<TEntity>()
                .Where(predicate)
                .ToListAsync();
        }
        public async Task<TEntity?> GetByIdAsync(int id) => await _context.Set<TEntity>().FindAsync(id);


        public void Update(TEntity entity) => _context.Set<TEntity>().Update(entity);
    }
}
