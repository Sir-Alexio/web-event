﻿using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WebEvent.API.Context;
using WebEvent.API.Repository.Abstract;

namespace WebEvent.API.Repository.Base
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected readonly ApplicationContext _db;

        public RepositoryBase(ApplicationContext db)
        {
            _db = db;
        }
        public async Task Create(T entity)
        {
            await _db.Set<T>().AddAsync(entity);
        }

        public async Task Delete(T entity)
        {
            T? entityToRemove = await _db.Set<T>().FindAsync(entity);

            if (entityToRemove != null)
            {
                _db.Set<T>().Remove(entityToRemove);
            }
        }

        public virtual async Task<IQueryable<T>> GetAll(bool trackChanges)
        {
            IQueryable<T> entities;

            if (!trackChanges)
            {
                entities = await Task.Run(() => _db.Set<T>().AsQueryable().AsNoTracking());

            }
            else
            {
                entities = await Task.Run(() => _db.Set<T>().AsQueryable());

            }

            return entities;
        }

        public async Task<IQueryable<T>> GetByCondition(Expression<Func<T, bool>> expression, bool trackChanges)
        {
            IQueryable<T> entities;

            if (!trackChanges)
            {
                entities = await Task.Run(() => _db.Set<T>().Where(expression).AsQueryable().AsNoTracking());
            }
            else
            {
                entities = await Task.Run(() => _db.Set<T>().Where(expression).AsQueryable());
            }

            return entities; ;
        }

        public async Task Update(T entity)
        {
            await Task.Run(() => _db.Set<T>().Update(entity));
        }
    }
}
