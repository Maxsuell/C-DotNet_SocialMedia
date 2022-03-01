using CloudinaryDotNet.Actions;

namespace api.Interfaces
{
    public interface IPhotoService
    {
        Task<ImageUploadResult> AddPhotoAsync(IFormFile file);

        Task<DeletionResult> DeletePhotAsync(String publicId);
        
    }
}