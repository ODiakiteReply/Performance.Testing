using System;
using AZWebAppCDB1.Models;

namespace AZWebAppCDB1.DataAccess.Repositories.CDRepositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User> CreateAsync(User user);
        Task<User> UpdateAsync(User user);
        Task<User?> GetByIdAsync(string id);
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetByUsernameAsync(string userName);
        Task<string?> DeleteAsync(string id, string partitionKey);
    }
}
