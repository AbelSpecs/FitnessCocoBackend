using InsightCore.Domain.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace InsightCore.Application.Interface.Persistence
{
    public interface IStudentsRepository : IGenericRepository<Student>
    {
        Task<Student> GetByIdAsync(int id);
        Task<Student> GetByUserIdAsync(int userId);
        Task<Student> InsertAsync(Student entity);
    }
}
