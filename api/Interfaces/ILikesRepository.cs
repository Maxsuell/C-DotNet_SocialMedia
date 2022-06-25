using api.DTO;
using api.Entities;
using api.Helpers;

namespace api.Interfaces
{
    public interface ILikesRepository
    {
        Task<UserLike> GetUserLike(int sourceuserId, int likedUserId);

        Task<AppUser> GetUserWithLikes(int userId);

        Task<PagedList<LikeDto>> GetUserLike(LikesParams likesParams);
    }
}