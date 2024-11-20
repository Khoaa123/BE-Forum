using Be_Voz_Clone.src.Model.Entities;
using Be_Voz_Clone.src.Shared.Core.Exceptions;
using Be_Voz_Clone.src.Shared.Database.DbContext;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace Be_Voz_Clone.src.Services.Implement;

public class CloudinaryService : ICloudinaryService
{
    private readonly Cloudinary _cloudinary;
    private readonly AppDbContext _context;

    public CloudinaryService(IConfiguration configuration, AppDbContext context)
    {
        var account = new Account(
            configuration["Cloudinary:CloudName"],
            configuration["Cloudinary:ApiKey"],
            configuration["Cloudinary:ApiSecret"]
        );
        _cloudinary = new Cloudinary(account);
        _context = context;
    }

    public async Task<string> UploadAvatarAsync(IFormFile file, string folder, string userId)
    {
        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(file.FileName, file.OpenReadStream()),
            Folder = folder,
            PublicId = userId
        };
        var uploadResult = await _cloudinary.UploadAsync(uploadParams);
        if (uploadResult.Error != null) throw new BadRequestException(uploadResult.Error.Message);
        var url = uploadResult.SecureUrl.ToString();
        return url;
    }

    public async Task<List<string>> UploadImagesAsync(List<IFormFile> files, string folder, string name)
    {
        var urls = new List<string>();
        foreach (var file in files)
        {
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, file.OpenReadStream()),
                Folder = folder
            };
            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            if (uploadResult.Error != null) throw new BadRequestException(uploadResult.Error.Message);
            var url = uploadResult.SecureUrl.ToString();
            urls.Add(url);
            var emojiAndSticker = new EmojiAndSticker
            {
                Url = url,
                Folder = folder,
                Name = name
            };
            _context.EmojiAndStickers.Add(emojiAndSticker);
        }

        await _context.SaveChangesAsync();
        return urls;
    }
}