using AutoMapper;
using Common.Extensions.Enum;
using AZWebAppCDB1.Common.Domain;
using AZWebAppCDB1.Business.Services.Interfaces;
using AZWebAppCDB1.DataAccess.Repositories.CDRepositories.Interfaces;

namespace AZWebAppCDB1.Business.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;

        private readonly IUserRepository _userRepository;

        public UserService(IMapper mapper, IUserRepository userRepository) 
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task<UserDTO?> CreateUserAsync(UserDTO user)
        {
            if (!string.IsNullOrWhiteSpace(user.Email) && !string.IsNullOrWhiteSpace(user.Username))
            {
                var existingUser = await _userRepository.GetByIdAsync(user.Username);
                if (existingUser != null)
                {
                    throw new Exception("User already exists!");
                }
                var userToCreate = _mapper.Map<Models.User>(user);
                var createdUser = await _userRepository.CreateAsync(userToCreate);
                return _mapper.Map<UserDTO>(createdUser);
            }
            return null;
        }

        public async Task<string> DeleteUserAsync(string id)
        {
            Models.User? existingUser = await _userRepository.GetByIdAsync(id);
            if (existingUser == null) return string.Empty;
            return await _userRepository.DeleteAsync(existingUser.Id, existingUser.RoleId) ?? string.Empty;
        }

        public Task<IEnumerable<UserDTO>> GetAllUsersAsync() => 
            Task.FromResult(_mapper.Map<IEnumerable<UserDTO>>(_userRepository.GetAllAsync().Result));

        public async Task<UserDTO?> GetUserByIdAsync(string id)
        {
            return _mapper.Map<UserDTO>(await _userRepository.GetByIdAsync(id));
        }

        public async Task<UserDTO?> GetUserByUsernameAsync(string username)
        {
            return _mapper.Map<UserDTO>(await _userRepository.GetByUsernameAsync(username));
        }

        public async Task<UserDTO?> UpdateUserAsync(UserDTO user)
        {
            Models.User? existingUser = await _userRepository.GetByIdAsync(user.Id);
            if (existingUser == null) return null;
            if (!string.IsNullOrWhiteSpace(user.Username) && existingUser.Username != user.Username)
            {
                var existingUserWithNewUsername = await _userRepository.GetByUsernameAsync(user.Username);
                if (existingUserWithNewUsername != null)
                {
                    throw new Exception("User already exists!");
                }
                existingUser.Username = user.Username;
            }
            existingUser.Email = user.Email;
            if (!string.IsNullOrWhiteSpace(user.LastName)) existingUser.LastName = user.LastName;
            if (!string.IsNullOrWhiteSpace(user.FirstName)) existingUser.FirstName = user.FirstName;
            if (!string.IsNullOrWhiteSpace(user.PhoneNumber)) existingUser.PhoneNumber = user.PhoneNumber;
            existingUser.RoleId = user.Role.ToInt().ToString();
            return _mapper.Map<UserDTO>(await _userRepository.UpdateAsync(existingUser));
        }
    }
}
