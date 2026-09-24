
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsTower.infrastructure.Contract
{
   
        public interface IUnitOfWork
        {
            IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : class;
            Task<int> SaveChangesAsync();
        }
    }

