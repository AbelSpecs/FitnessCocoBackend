using InsightCore.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InsightCore.Application.Interface.Persistence
{
    /// <summary>
    /// Repositorio de pagos con métodos de consulta específicos
    /// del dominio de pagos (filtros por usuario, moneda, estado, etc.).
    /// </summary>
    public interface IPaymentsRepository : IGenericRepository<Payment>
    {
        /// <summary>Obtiene un pago por su Id interno.</summary>
        Task<Payment?> GetByIdAsync(int id);

        /// <summary>
        /// Persiste una nueva entidad Payment y devuelve la entidad con el Id generado.
        /// Sobrescribe la firma de IGenericRepository para devolver la entidad completa.
        /// </summary>
        Task<Payment> InsertAsync(Payment entity);

        /// <summary>
        /// Consulta paginada de pagos con filtros opcionales.
        /// </summary>
        Task<(IEnumerable<Payment> Items, int TotalCount)> GetFilteredAsync(
            int? userId,
            int? studentId,
            string? currency,
            string? status,
            System.DateTime? from,
            System.DateTime? to,
            int page,
            int pageSize);
    }
}
