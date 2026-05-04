using InsightCore.Application.Interface.Persistence;
using InsightCore.Domain.Entities;
using InsightCore.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsightCore.Persistence.Repositories
{
    public class TransactionsRepository : ITransactionsRepository
    {
        private readonly ApplicationDbContext _context;
        public TransactionsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Transaction>> GetTransactionsByUserIdAsync(int userId)
        {
            try
            {
                // Creamos la consulta base
                var query = _context.Set<Transaction>().AsNoTracking();

                // Si userId es distinto de 0, filtramos. 
                // Si es 0, no entra al IF y trae todo por defecto.
                if (userId != 0)
                {
                    query = query.Where(u => u.usuario_id == userId);
                }

                return await query.ToListAsync();

            }
            catch (Exception ex)
            {
                throw new Exception($"Error al buscar el usuario por email: {ex.Message}");
            }
        }
    }

}
