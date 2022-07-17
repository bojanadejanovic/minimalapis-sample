using UsersAndGroupsAPI.Models;

namespace UsersAndGroupsAPI.Interfaces
{
    public interface IUserService
    {
        Task<User?> GetUserById(int id);
        Task<UserModel?> GetUser(int id);
        Task<User?> GetUserByName(string name);
        Task<User?> CreateNewUser(CreateUserRequest request);
        Task<List<User>> GetUsers();

        Task<UserGroup?> AddUserToGroup(int userId, int groupId);
    }
}
