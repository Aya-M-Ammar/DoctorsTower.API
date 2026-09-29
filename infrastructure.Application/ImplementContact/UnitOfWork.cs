
using DoctorsTower.infrastructure.Contract;
using DoctorsTower.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsTower.Application.ImplementContact
{
        public class UnitOfWork : IUnitOfWork
        {
            private readonly DoctorsTowerDbContext _context;
            private readonly Dictionary<Type, object> _repositories = new Dictionary<Type, object>();

            public UnitOfWork(DoctorsTowerDbContext context)
            {
                _context = context;
            }

            public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : class
            {
                if (_repositories.TryGetValue(typeof(TEntity), out var repo))
                {
                    return (IGenericRepository<TEntity>)repo;
                }

                var repositoryInstance = new GenericRepository<TEntity>(_context);
                _repositories.Add(typeof(TEntity), repositoryInstance);

                return repositoryInstance;
            }

            public Task<int> SaveChangesAsync()
            {
                return _context.SaveChangesAsync();
            }
        }
    }
