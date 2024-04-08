using AZWebAppCDB1.Common.Domain;

namespace AZWebAppCDB1.Business.Services.Interfaces
{
    public interface IUserService
    {
        Task<string> DeleteUserAsync(string id);
        Task<UserDTO?> GetUserByIdAsync(string id);
        Task<UserDTO?> CreateUserAsync(UserDTO user);
        Task<UserDTO?> UpdateUserAsync(UserDTO user);
        Task<IEnumerable<UserDTO>> GetAllUsersAsync();
        Task<UserDTO?> GetUserByUsernameAsync(string username);
    }
}
