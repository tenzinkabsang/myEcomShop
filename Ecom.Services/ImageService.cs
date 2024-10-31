namespace Ecom.Services;

public class ImageService : IImageService
{
    public Task<string> GetImageLocalPathAsync(string fileName)
    {
        return Task.FromResult($"images/products/{fileName}");
    }

    public Task<string> GetImageUriAsync(string imageName)
    {
        return Task.FromResult($"images/products/{imageName}");
    }
}
