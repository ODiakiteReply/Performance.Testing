using System;
using Performance.Testing.Models;


namespace Performance.Testing.DataAccess.Repositories.CDRepositories.Interfaces
{
    public interface IPostRepository
    {
        Task<Post> CreateAsync(Post post);
        Task<Post> UpdateAsync(Post post);
        Task<Post?> GetByIdAsync(string id);
        Task<IEnumerable<Post>> GetAllAsync();
        Task<IEnumerable<Post>> GetByUserAsync(string userId);
        Task<string?> DeleteAsync(string id, string partitionKey);
    }
}
