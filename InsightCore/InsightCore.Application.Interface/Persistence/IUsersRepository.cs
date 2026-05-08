using InsightCore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsightCore.Application.Interface.Persistence
{
    public interface IUsersRepository : IGenericRepository<User>
    {
        Task<User> Authenticate(string username, string password);
        Task<User> RegisterUser(User user);
        Task<User> GetByEmailAsync(string email);
        Task<User> GetByUserNameAsync(string userName);
        Task<User> GetByIdAsync(int id);
        Task<(Student Student, Coach Coach)> GetUserDetailByUserIdAsync(int userId);
    }
}
