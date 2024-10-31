namespace Ecom.Services;

public interface IImageService
{
    Task<string> GetImageUriAsync(string imageName);
    Task<string> GetImageLocalPathAsync(string fileName);
}
