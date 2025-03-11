using Performance.Testing.Common.Domain;


namespace Performance.Testing.Business.Services.Interfaces
{
    public interface ICommentService
    {
        Task<string> DeleteAsync(string id);
        Task<CommentDTO?> GetByIdAsync(string id);
        Task<IEnumerable<CommentDTO>> GetAllAsync();
        Task<CommentDTO?> CreateAsync(CommentDTO comment);
        Task<CommentDTO?> UpdateAsync(CommentDTO comment);
        Task<IEnumerable<CommentDTO>> GetByPostAsync(string psotId);
    }
}
