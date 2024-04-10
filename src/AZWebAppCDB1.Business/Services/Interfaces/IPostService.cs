﻿using AZWebAppCDB1.Common.Domain;


namespace AZWebAppCDB1.Business.Services.Interfaces
{
    public interface IPostService
    {
        Task<string> DeleteAsync(string id);
        Task<PostDTO?> GetByIdAsync(string id);
        Task<IEnumerable<PostDTO>> GetAllAsync();
        Task<PostDTO?> CreateAsync(PostDTO post);
        Task<PostDTO?> UpdateAsync(PostDTO post);
        Task<IEnumerable<PostDTO>> GetByUserAsync(string userId);
    }
}
