using AutoMapper;
using AZWebAppCDB1.Models;
using AZWebAppCDB1.Common.Domain;
using AZWebAppCDB1.Business.Services.Interfaces;
using AZWebAppCDB1.DataAccess.Repositories.CDRepositories.Interfaces;


namespace AZWebAppCDB1.Business.Services.Implementations
{
    public class PostService : IPostService
    {

        private readonly IMapper _mapper;

        private readonly IPostRepository _postRepository;
        private readonly IUserRepository _userRepository;

        public PostService(IMapper mapper, IPostRepository postRepository, IUserRepository userRepository)
        {
            _mapper = mapper;
            _postRepository = postRepository;
            _userRepository=userRepository;
        }

        public async Task<PostDTO?> CreateAsync(PostDTO post)
        {
            if (!string.IsNullOrWhiteSpace(post.UserId) &&
                !string.IsNullOrWhiteSpace(post.Title) &&
                !string.IsNullOrWhiteSpace(post.Content))
            {
                var existingUser = await _userRepository.GetByIdAsync(post.UserId)
                    ?? throw new Exception("User Not Found!");
                var postToCreate = _mapper.Map<Post>(post);
                postToCreate.CreatedBy = $"{existingUser.FirstName} {existingUser.LastName}";
                postToCreate.CreatedAt = DateTime.UtcNow;
                postToCreate.UpdatedAt = null;
                var createdUser = await _postRepository.CreateAsync(postToCreate);
                return _mapper.Map<PostDTO>(createdUser);
            }
            return null;
        }

        public async Task<string> DeleteAsync(string id)
        {
            Post? existingPost = await _postRepository.GetByIdAsync(id);
            if (existingPost is null) return string.Empty;
            return await _postRepository.DeleteAsync(existingPost.Id, existingPost.UserId) ?? string.Empty;
        }

        public Task<IEnumerable<PostDTO>> GetAllAsync() => 
            Task.FromResult(_mapper.Map<IEnumerable<PostDTO>>(_postRepository.GetAllAsync().Result));

        public async Task<PostDTO?> GetByIdAsync(string id)
        {
            return _mapper.Map<PostDTO>(await _postRepository.GetByIdAsync(id));
        }

        public async Task<IEnumerable<PostDTO>> GetByUserAsync(string userId)
        {
            return _mapper.Map<IEnumerable<PostDTO>>(await _postRepository.GetByUserAsync(userId));
        }

        public async Task<PostDTO?> UpdateAsync(PostDTO post)
        {
            Post? existingPost = await _postRepository.GetByIdAsync(post.Id);
            if (existingPost == null) return null;
            if (!string.IsNullOrWhiteSpace(post.Title)) existingPost.Title = post.Title;
            if (!string.IsNullOrWhiteSpace(post.Content)) existingPost.Content = post.Content;
            if (!string.IsNullOrWhiteSpace(post.UpdatedBy)) existingPost.UpdatedBy = post.UpdatedBy;
            existingPost.UpdatedAt = DateTime.UtcNow;
            return _mapper.Map<PostDTO>(await _postRepository.UpdateAsync(existingPost));
        }
    }
}
