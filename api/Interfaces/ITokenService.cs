using api.Entities;

namespace api.Interfaces
{
    public interface ITokenService
    {
         string CreateToken(AppUser user);
    }
}