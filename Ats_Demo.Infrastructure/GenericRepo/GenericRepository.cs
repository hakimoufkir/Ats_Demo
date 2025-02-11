using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;
using System.Linq;
using Ats_Demo.Infrastructure.Data;
using Ats_Demo.Application.IGenericRepository;

namespace Ats_Demo.Infrastructure.GenericRepo
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> DbSet;

        public GenericRepository(ApplicationDbContext db)
        {
            _db = db;
            DbSet = _db.Set<T>();
        }

        public async Task<List<T>?> GetAllAsync(Expression<Func<T, bool>>? filter)
        {
            IQueryable<T> query = DbSet;
            if (filter != null) { query = query.Where(filter); }
            return await query.ToListAsync();
        }

        public async Task<T?> GetAsync(Expression<Func<T, bool>>? filter, bool tracked = true)
        {
            IQueryable<T> query = DbSet;
            if (!tracked) { query = query.AsNoTracking(); }
            if (filter != null) { query = query.Where(filter); }
            return await query.FirstOrDefaultAsync();
        }

        public virtual async Task CreateAsync(T entity)
        {
            await DbSet.AddAsync(entity);
            await SaveAsync();
        }
        public virtual async Task UpdateAsync(T entity)
        {
            _db.Set<T>().Update(entity);
            await _db.SaveChangesAsync();
        }

        public virtual async Task RemoveAsync(T entity)
        {
            DbSet.Remove(entity);
            await SaveAsync();
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }

    }
}
