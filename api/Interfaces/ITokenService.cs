using api.Entities;

namespace api.Interfaces
{
    public interface ITokenService
    {
         Task<string> CreateToken(AppUser user);
    }
}