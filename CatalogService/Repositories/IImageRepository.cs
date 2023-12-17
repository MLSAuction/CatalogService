using ImageService.Models;

namespace ImageService.Repositories
{
    public interface IImageRepository
    {
        ImageDTO? GetImage(int imageId);
        ImageDTO? UploadImage(FormFile image);
        void DeleteImage(int imageId);
    }
}
