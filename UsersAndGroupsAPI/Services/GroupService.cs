using Microsoft.EntityFrameworkCore;
using UsersAndGroupsAPI.Db;
using UsersAndGroupsAPI.Interfaces;
using UsersAndGroupsAPI.Models;

namespace UsersAndGroupsAPI.Services
{
    public class GroupService : IGroupService
    {
        private readonly UsersAndGroupsContext _dbContext;
        public GroupService(UsersAndGroupsContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Group?> CreateNewGroup(CreateGroupRequest req)
        {
            var group = new Group() { Name = req.Name };
            _dbContext.Group.Add(group);
            await _dbContext.SaveChangesAsync();
            return group;
        }

        public async Task<List<Group>> GetAllGroups()
        {
            return await _dbContext.Group.ToListAsync();
        }

        public async Task<Group?> GetGroupById(int id)
        {
            return await _dbContext.Group.FindAsync(id);
        }
    }
}
