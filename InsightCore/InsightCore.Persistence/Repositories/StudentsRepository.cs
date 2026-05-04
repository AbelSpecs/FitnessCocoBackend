using InsightCore.Application.Interface.Persistence;
using InsightCore.Domain.Entities;
using InsightCore.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace InsightCore.Persistence.Repositories
{
    public class StudentsRepository : IStudentsRepository
    {
        private readonly ApplicationDbContext _context;

        public StudentsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Student> GetByIdAsync(int id)
        {
            return await _context.Set<Student>().AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Student> GetByUserIdAsync(int userId)
        {
            return await _context.Set<Student>().AsNoTracking().FirstOrDefaultAsync(s => s.UserId == userId);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.Set<Student>().FindAsync(id);
            if (entity == null) return false;
            _context.Set<Student>().Remove(entity);
            var rows = await _context.SaveChangesAsync();
            return rows > 0;
        }

        public bool Delete(string id)
        {
            if (!int.TryParse(id, out var intId)) return false;
            var entity = _context.Set<Student>().Find(intId);
            if (entity == null) return false;
            _context.Set<Student>().Remove(entity);
            return _context.SaveChanges() > 0;
        }

        public IEnumerable<Student> GetAll()
        {
            return _context.Set<Student>().AsNoTracking();
        }

        public async Task<IEnumerable<Student>> GetAllAsync()
        {
            return await _context.Set<Student>().AsNoTracking().ToListAsync();
        }

        public IEnumerable<Student> GetAllWithPagination(int pageNumber, int pageSize)
        {
            return _context.Set<Student>().AsNoTracking().Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }

        public async Task<IEnumerable<Student>> GetAllWithPaginationAsync(int pageNumber, int pageSize)
        {
            return await _context.Set<Student>().AsNoTracking().Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public int Count()
        {
            return _context.Set<Student>().Count();
        }

        public async Task<int> CountAsync()
        {
            return await _context.Set<Student>().CountAsync();
        }

        public Student Get(string id)
        {
            if (!int.TryParse(id, out var intId)) return null;
            return _context.Set<Student>().Find(intId);
        }

        public async Task<Student> GetAsync(string id)
        {
            if (!int.TryParse(id, out var intId)) return null;
            return await _context.Set<Student>().FindAsync(intId);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            if (!int.TryParse(id, out var intId)) return false;
            return await DeleteAsync(intId);
        }

        public bool Insert(Student entity)
        {
            _context.Set<Student>().Add(entity);
            return _context.SaveChanges() > 0;
        }

        public async Task<bool> InsertAsync(Student entity)
        {
            await _context.Set<Student>().AddAsync(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public bool Update(Student entity)
        {
            _context.Set<Student>().Update(entity);
            return _context.SaveChanges() > 0;
        }

        public async Task<bool> UpdateAsync(Student entity)
        {
            _context.Set<Student>().Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
