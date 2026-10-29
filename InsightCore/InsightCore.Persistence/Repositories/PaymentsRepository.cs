using InsightCore.Application.Interface.Persistence;
using InsightCore.Domain.Entities;
using InsightCore.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InsightCore.Persistence.Repositories
{
    /// <summary>
    /// Implementación del repositorio de pagos usando EF Core.
    /// Sigue el mismo patrón que StudentsRepository.
    /// </summary>
    public class PaymentsRepository : IPaymentsRepository
    {
        private readonly ApplicationDbContext _context;

        public PaymentsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // ── Métodos específicos del dominio ──────────────────────────────────────

        public async Task<Payment?> GetByIdAsync(int id)
        {
            return await _context.Set<Payment>()
                                 .AsNoTracking()
                                 .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Payment> InsertAsync(Payment entity)
        {
            await _context.Set<Payment>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<(IEnumerable<Payment> Items, int TotalCount)> GetFilteredAsync(
            int? userId,
            int? studentId,
            string? currency,
            string? status,
            DateTime? from,
            DateTime? to,
            int page,
            int pageSize)
        {
            var query = _context.Set<Payment>().AsNoTracking();

            if (userId.HasValue)
                query = query.Where(p => p.UserId == userId.Value);

            //if (studentId.HasValue)
            //    query = query.Where(p => p.StudentId == studentId.Value);

            if (!string.IsNullOrWhiteSpace(currency))
                query = query.Where(p => p.Currency.ToString() == currency);

            if (!string.IsNullOrWhiteSpace(status))
                query = query.Where(p => p.Status.ToString() == status);

            if (from.HasValue)
                query = query.Where(p => p.CreatedAt >= from.Value);

            if (to.HasValue)
                query = query.Where(p => p.CreatedAt <= to.Value);

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(p => p.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        // ── IGenericRepository<Payment> ──────────────────────────────────────────

        public bool Insert(Payment entity)
        {
            _context.Set<Payment>().Add(entity);
            return _context.SaveChanges() > 0;
        }

        public bool Update(Payment entity)
        {
            _context.Set<Payment>().Update(entity);
            return _context.SaveChanges() > 0;
        }

        public async Task<bool> UpdateAsync(Payment entity)
        {
            _context.Set<Payment>().Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public bool Delete(string id)
        {
            if (!int.TryParse(id, out var intId)) return false;
            var entity = _context.Set<Payment>().Find(intId);
            if (entity == null) return false;
            _context.Set<Payment>().Remove(entity);
            return _context.SaveChanges() > 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            if (!int.TryParse(id, out var intId)) return false;
            var entity = await _context.Set<Payment>().FindAsync(intId);
            if (entity == null) return false;
            _context.Set<Payment>().Remove(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public Payment Get(string id)
        {
            if (!int.TryParse(id, out var intId)) return null!;
            return _context.Set<Payment>().Find(intId)!;
        }

        public async Task<Payment> GetAsync(string id)
        {
            if (!int.TryParse(id, out var intId)) return null!;
            return (await _context.Set<Payment>().FindAsync(intId))!;
        }

        public IEnumerable<Payment> GetAll()
        {
            return _context.Set<Payment>().AsNoTracking();
        }

        public async Task<IEnumerable<Payment>> GetAllAsync()
        {
            return await _context.Set<Payment>().AsNoTracking().ToListAsync();
        }

        public IEnumerable<Payment> GetAllWithPagination(int pageNumber, int pageSize)
        {
            return _context.Set<Payment>()
                           .AsNoTracking()
                           .OrderByDescending(p => p.CreatedAt)
                           .Skip((pageNumber - 1) * pageSize)
                           .Take(pageSize);
        }

        public async Task<IEnumerable<Payment>> GetAllWithPaginationAsync(int pageNumber, int pageSize)
        {
            return await _context.Set<Payment>()
                                 .AsNoTracking()
                                 .OrderByDescending(p => p.CreatedAt)
                                 .Skip((pageNumber - 1) * pageSize)
                                 .Take(pageSize)
                                 .ToListAsync();
        }

        public int Count() => _context.Set<Payment>().Count();

        public async Task<int> CountAsync() => await _context.Set<Payment>().CountAsync();

        // Explicit interface implementations to satisfy IGenericRepository contract
        Task<bool> IGenericRepository<Payment>.InsertAsync(Payment entity)
        {
            return Task.FromResult(Insert(entity));
        }
    }
}
