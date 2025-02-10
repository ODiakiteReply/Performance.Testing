using AutoMapper;
using Performance.Testing.Models;
using Performance.Testing.Common.Domain;
using Performance.Testing.Business.Services.Interfaces;
using Performance.Testing.DataAccess.Repositories.CDRepositories.Interfaces;


namespace Performance.Testing.Business.Services.Implementations
{
    public class CommentService : ICommentService
    {
        private readonly IMapper _mapper;

        private readonly IUserRepository _userRepository;
        private readonly IPostRepository _postRepository;
        private readonly ICommentRepository _commentRepository;

        public CommentService(
            IMapper mapper,
            IUserRepository userRepository,
            IPostRepository postRepository, 
            ICommentRepository commentRepository)
        {
            _mapper = mapper;
            _postRepository = postRepository;
            _userRepository=userRepository;
            _commentRepository = commentRepository;
        }

        public async Task<CommentDTO?> CreateAsync(CommentDTO comment)
        {
            User? existingUser = await _userRepository.GetByIdAsync(comment.UserId ?? string.Empty) 
                ?? throw new Exception("User Not Found!");
            Post? existingPost = await _postRepository.GetByIdAsync(comment.PostId ?? string.Empty) 
                ?? throw new Exception("Post Not Found!");
            if (string.IsNullOrWhiteSpace(comment.Content))
            {
                throw new Exception("Comment Content is required!");
            }
            var commentToCreate = _mapper.Map<Comment>(comment);
            commentToCreate.CreatedBy = $"{existingUser.FirstName} {existingUser.LastName}";
            return _mapper.Map<CommentDTO>(await _commentRepository.CreateAsync(commentToCreate));
        }

        public async Task<string> DeleteAsync(string id)
        {
            Comment? existingComment = await _commentRepository.GetByIdAsync(id);
            if (existingComment == null) return string.Empty;
            return await _commentRepository.DeleteAsync(existingComment.Id, existingComment.PostId) ?? string.Empty;
        }

        public Task<IEnumerable<CommentDTO>> GetAllAsync() => 
            Task.FromResult(_mapper.Map<IEnumerable<CommentDTO>>(_commentRepository.GetAllAsync().Result));

        public async Task<CommentDTO?> GetByIdAsync(string id)
        {
            return _mapper.Map<CommentDTO>(await _commentRepository.GetByIdAsync(id));
        }

        public async Task<IEnumerable<CommentDTO>> GetByPostAsync(string postId)
        {
            return _mapper.Map<IEnumerable<CommentDTO>>(await _commentRepository.GetByPostAsync(postId));
        }

        public async Task<CommentDTO?> UpdateAsync(CommentDTO comment)
        {
            Comment? existingComment = await _commentRepository.GetByIdAsync(comment.Id ?? string.Empty)
                ?? throw new Exception("Comment Not Found!");
            existingComment.Content = comment.Content;
            existingComment.UpdatedAt = DateTime.UtcNow;
            return _mapper.Map<CommentDTO>(await _commentRepository.UpdateAsync(existingComment));
        }
    }
}
