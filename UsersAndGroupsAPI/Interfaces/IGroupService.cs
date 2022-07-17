using UsersAndGroupsAPI.Models;

namespace UsersAndGroupsAPI.Interfaces
{
    public interface IGroupService
    {
        Task<Group?> GetGroupById(int id);
        Task<Group?> CreateNewGroup(CreateGroupRequest req);

        Task<List<Group>> GetAllGroups();
    }
}
