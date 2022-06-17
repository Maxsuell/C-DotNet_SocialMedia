using api.DTO;
using api.Entities;
using API.Helpers;

namespace api.Interfaces
{
    public interface IUserRepository
    {
         void Update(AppUser user);
         Task<bool> SaveAllAsync();
         Task<IEnumerable<AppUser>> GetUsersAsync();
         Task<AppUser> GetUserByIdAsync(int id);
         Task<AppUser> GetUserByUsernameAsync(string username);
         Task<MemberDto> GetMemberAsync(string username);
         Task<PagedList<MemberDto>> GetMembersAsync (UserParams userParams);
    }
}