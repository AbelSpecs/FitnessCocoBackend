using InsightCore.Application.Interface.Persistence;
using InsightCore.Domain.Entities;
using InsightCore.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsightCore.Persistence.Repositories
{
    public class CitiesRepository : IGenericRepository<City>
    {
        private readonly ApplicationDbContext _context;

        public CitiesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool Delete(string id)
        {
            if (!int.TryParse(id, out var intId)) return false;
            var entity = _context.Set<City>().Find(intId);
            if (entity == null) return false;
            _context.Set<City>().Remove(entity);
            return _context.SaveChanges() > 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            if (!int.TryParse(id, out var intId)) return false;
            var entity = await _context.Set<City>().FindAsync(intId);
            if (entity == null) return false;
            _context.Set<City>().Remove(entity);
            var rows = await _context.SaveChangesAsync();
            return rows > 0;
        }

        public IEnumerable<City> GetAll()
        {
            return _context.Set<City>().AsNoTracking();
        }

        public async Task<IEnumerable<City>> GetAllAsync()
        {
            return await _context.Set<City>().AsNoTracking().ToListAsync();
        }

        public IEnumerable<City> GetAllWithPagination(int pageNumber, int pageSize)
        {
            return _context.Set<City>().AsNoTracking().Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }

        public async Task<IEnumerable<City>> GetAllWithPaginationAsync(int pageNumber, int pageSize)
        {
            return await _context.Set<City>().AsNoTracking().Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public City Get(string id)
        {
            if (!int.TryParse(id, out var intId)) return null;
            return _context.Set<City>().Find(intId);
        }

        public async Task<City> GetAsync(string id)
        {
            if (!int.TryParse(id, out var intId)) return null;

            return await _context.Cities
            .AsNoTracking()
            .FirstAsync(c => c.Id == intId);
        }

        public int Count()
        {
            return _context.Set<City>().Count();
        }

        public async Task<int> CountAsync()
        {
            return await _context.Set<City>().CountAsync();
        }

        public bool Insert(City entity)
        {
            _context.Set<City>().Add(entity);
            return _context.SaveChanges() > 0;
        }

        public async Task<bool> InsertAsync(City entity)
        {
            await _context.Set<City>().AddAsync(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public bool Update(City entity)
        {
            _context.Set<City>().Update(entity);
            return _context.SaveChanges() > 0;
        }

        public async Task<bool> UpdateAsync(City entity)
        {
            _context.Set<City>().Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
