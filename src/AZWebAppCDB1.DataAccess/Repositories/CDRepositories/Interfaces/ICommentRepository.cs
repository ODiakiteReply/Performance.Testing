using System;
using Performance.Testing.Models;


namespace Performance.Testing.DataAccess.Repositories.CDRepositories.Interfaces
{
    public interface ICommentRepository
    {
        Task<Comment?> GetByIdAsync(string id);
        Task<IEnumerable<Comment>> GetAllAsync();
        Task<Comment> CreateAsync(Comment comment);
        Task<Comment> UpdateAsync(Comment comment);
        Task<IEnumerable<Comment>> GetByPostAsync(string postId);
        Task<string?> DeleteAsync(string id, string partitionKey);
    }
}
