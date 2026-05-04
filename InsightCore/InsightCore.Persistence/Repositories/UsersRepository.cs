using Dapper;
using InsightCore.Application.Interface.Persistence;
using InsightCore.Domain.Entities;
using InsightCore.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace InsightCore.Persistence.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly ApplicationDbContext _context;
        public UsersRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<User> Authenticate(string userName, string password)
        {

            try
            {
                var user = await _context
               .Set<User>()
               .FirstOrDefaultAsync(user => user.UserName == userName);

                return user;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<User> RegisterUser(User user)
        {
            try
            {
                // 1. Agregamos la entidad al Change Tracker de EF Core
                await _context.Set<User>().AddAsync(user);

                // 2. Persistimos los cambios en la base de datos
                await _context.SaveChangesAsync();

                return user;
            }
            catch (DbUpdateException ex)
            {
                // Error específico de base de datos (ej. duplicados si no se validó antes)
                throw new Exception("Error al persistir el usuario en la base de datos.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error inesperado: {ex.Message}");
            }
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            try
            {
                // Usamos FirstOrDefaultAsync para buscar por el campo Email
                // .AsNoTracking() es una buena práctica aquí si solo vas a leer el dato para validar
                return await _context.Set<User>()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.Email == email);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al buscar el usuario por email: {ex.Message}");
            }
        }

        public async Task<User> GetByUserNameAsync(string userName)
        {
            try
            {
                return await _context.Set<User>()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.UserName == userName || u.Email == userName);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener el usuario {userName} desde la base de datos.", ex);
            }
        }

        public async Task<bool> UpdateAsync(User entity)
        {
            try
            {
                // EF Core rastrea la entidad, pero esto asegura que se marque como modificada
                _context.Set<User>().Update(entity);

                // Es vital que se guarden los cambios. 
                // Si tu Unit of Work maneja el SaveChanges, puedes omitir la siguiente línea.
                var rowsAffected = await _context.SaveChangesAsync();

                // Retornamos true si se actualizó al menos una fila
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al actualizar el usuario {entity.UserName}", ex);
            }
        }


        public bool Insert(User entity)
        {
            throw new NotImplementedException();
        }

        public bool Update(User entity)
        {
            throw new NotImplementedException();
        }

        public bool Delete(string id)
        {
            throw new NotImplementedException();
        }

        public User Get(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetAllWithPagination(int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }

        public int Count()
        {
            throw new NotImplementedException();
        }

        public Task<bool> InsertAsync(User entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<User>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<User>> GetAllWithPaginationAsync(int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<int> CountAsync()
        {
            throw new NotImplementedException();
        }
    }
}
