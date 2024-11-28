using Be_Voz_Clone.src.Repositories;
using Be_Voz_Clone.src.Shared.Core.Exceptions;
using Be_Voz_Clone.src.UnitOfWork;

namespace Be_Voz_Clone.src.Services.Implement;

public class EmojiAndStickerService : IEmojiAndStickerService
{
    private readonly IUnitOfWork _unitOfWork;

    public EmojiAndStickerService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<string>> GetUrl(string name)
    {
        var emojiAndStickerRepository = _unitOfWork.GetRepository<IEmojiAndStickerRepository>();

        var urls = await emojiAndStickerRepository.GetUrlsByNameAsync(name);

        if (urls == null || !urls.Any())
        {
            throw new NotFoundException("Not found!");
        }

        return urls;
    }
}