namespace Be_Voz_Clone.src.Services
{
    public interface ICloudinaryService
    {
        Task<List<string>> UploadImagesAsync(List<IFormFile> files, string folder, string name);
    }
}
