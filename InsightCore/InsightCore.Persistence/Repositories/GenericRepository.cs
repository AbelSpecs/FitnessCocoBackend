using InsightCore.Application.Interface.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using InsightCore.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace InsightCore.Persistence.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public bool Delete(string id)
        {
            var entity = _dbSet.Find(id);
            if (entity == null) return false;
            _dbSet.Remove(entity);
            return _context.SaveChanges() > 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null) return false;
            _dbSet.Remove(entity);
            var rows = await _context.SaveChangesAsync();
            return rows > 0;
        }

        public IEnumerable<T> GetAll()
        {
            return _dbSet.AsNoTracking();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public IEnumerable<T> GetAllWithPagination(int pageNumber, int pageSize)
        {
            return _dbSet.AsNoTracking().Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }

        public async Task<IEnumerable<T>> GetAllWithPaginationAsync(int pageNumber, int pageSize)
        {
            return await _dbSet.AsNoTracking().Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public T Get(string id)
        {
            return _dbSet.Find(id);
        }

        public async Task<T> GetAsync(string id)
        {
            return await _dbSet.FindAsync(id);
        }

        public int Count()
        {
            return _dbSet.Count();
        }

        public async Task<int> CountAsync()
        {
            return await _dbSet.CountAsync();
        }

        public bool Insert(T entity)
        {
            _dbSet.Add(entity);
            return _context.SaveChanges() > 0;
        }

        public async Task<bool> InsertAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public bool Update(T entity)
        {
            _dbSet.Update(entity);
            return _context.SaveChanges() > 0;
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
