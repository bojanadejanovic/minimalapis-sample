using UsersAndGroupsAPI.Db;
using UsersAndGroupsAPI.Interfaces;
using UsersAndGroupsAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace UsersAndGroupsAPI.Services
{
    public class UserService : IUserService
    {
        private UsersAndGroupsContext _dbContext;

        public UserService(UsersAndGroupsContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UserGroup?> AddUserToGroup(int userId, int groupId)
        {
            var userInGroup = await _dbContext.UserGroup.Where(g => g.UserId == userId && g.GroupId == groupId).FirstOrDefaultAsync();
            if (userInGroup == null)
            {
                await _dbContext.UserGroup.AddAsync(new UserGroup() { UserId = userId, GroupId = groupId });
                await _dbContext.SaveChangesAsync();
                return userInGroup;
            }
            return null;
        }

        public async Task<User?> CreateNewUser(CreateUserRequest req)
        {
            var userDb = new User { Email = req.Email, Name = req.Email, Password = req.Password };
            try
            {
                await _dbContext.User.AddAsync(userDb);
                await _dbContext.SaveChangesAsync();
                if(_dbContext.Group.Find(req.GroupId) != null)
                {
                    await _dbContext.UserGroup.AddAsync(new UserGroup { UserId = userDb.Id, GroupId = req.GroupId });
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    throw new InvalidOperationException($"Group with id {req.GroupId} does not exist.");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw ex;
            }

          return userDb;
        }

        public async Task<UserModel?> GetUser(int id)
        {
            var user = await _dbContext.User.FindAsync(id);
            var group = await _dbContext.UserGroup.Where(x => x.UserId == id).FirstOrDefaultAsync();
            var groupName = await _dbContext.Group.FindAsync(group?.Id);
            return new UserModel() { Id = user?.Id, Name = user?.Name, Group = groupName?.Name };
        }

        public async Task<User?> GetUserById(int id)
        {
            return await _dbContext.User.FindAsync(id);
        }

        public async Task<User?> GetUserByName(string name)
        {
            return  await _dbContext.User.FirstOrDefaultAsync(x => x.Name == name);
        }

        public async Task<List<User>> GetUsers()
        {
            return await _dbContext.User.ToListAsync();
        }
    }
}
