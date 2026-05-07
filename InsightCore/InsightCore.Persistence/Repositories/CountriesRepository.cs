using InsightCore.Application.Interface.Persistence;
using InsightCore.Domain.Entities;
using InsightCore.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace InsightCore.Persistence.Repositories
{
    public class CountriesRepository : IGenericRepository<Country>
    {
        private readonly ApplicationDbContext _context;

        public CountriesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool Delete(string id)
        {
            if (!int.TryParse(id, out var intId)) return false;
            var entity = _context.Set<Country>().Find(intId);
            if (entity == null) return false;
            _context.Set<Country>().Remove(entity);
            return _context.SaveChanges() > 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            if (!int.TryParse(id, out var intId)) return false;
            var entity = await _context.Set<Country>().FindAsync(intId);
            if (entity == null) return false;
            _context.Set<Country>().Remove(entity);
            var rows = await _context.SaveChangesAsync();
            return rows > 0;
        }

        public IEnumerable<Country> GetAll()
        {
            return _context.Set<Country>().AsNoTracking();
        }

        public async Task<IEnumerable<Country>> GetAllAsync()
        {
            return await _context.Set<Country>().AsNoTracking().ToListAsync();
        }

        public IEnumerable<Country> GetAllWithPagination(int pageNumber, int pageSize)
        {
            return _context.Set<Country>().AsNoTracking().Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }

        public async Task<IEnumerable<Country>> GetAllWithPaginationAsync(int pageNumber, int pageSize)
        {
            return await _context.Set<Country>().AsNoTracking().Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public Country Get(string id)
        {
            if (!int.TryParse(id, out var intId)) return null;
            return _context.Set<Country>().Find(intId);
        }

        public async Task<Country> GetAsync(string id)
        {
            if (!int.TryParse(id, out var intId)) return null;

            return await _context.Countries
            .AsNoTracking()
            .FirstAsync(c => c.Id == intId);
        }

        public int Count()
        {
            return _context.Set<Country>().Count();
        }

        public async Task<int> CountAsync()
        {
            return await _context.Set<Country>().CountAsync();
        }

        public bool Insert(Country entity)
        {
            _context.Set<Country>().Add(entity);
            return _context.SaveChanges() > 0;
        }

        public async Task<bool> InsertAsync(Country entity)
        {
            await _context.Set<Country>().AddAsync(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public bool Update(Country entity)
        {
            _context.Set<Country>().Update(entity);
            return _context.SaveChanges() > 0;
        }

        public async Task<bool> UpdateAsync(Country entity)
        {
            _context.Set<Country>().Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
