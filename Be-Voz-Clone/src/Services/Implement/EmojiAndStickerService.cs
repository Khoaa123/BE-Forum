using Be_Voz_Clone.src.Repositories;
using Be_Voz_Clone.src.Shared.Core.Exceptions;

namespace Be_Voz_Clone.src.Services.Implement;

public class EmojiAndStickerService : IEmojiAndStickerService
{
    private readonly IEmojiAndStickerRepository _emojiAndStickerRepository;

    public EmojiAndStickerService(IEmojiAndStickerRepository emojiAndStickerRepository)
    {
        _emojiAndStickerRepository = emojiAndStickerRepository;
    }

    public async Task<List<string>> GetUrl(string name)
    {
        var urls = await _emojiAndStickerRepository.GetUrlsByNameAsync(name);

        if (urls == null || !urls.Any())
        {
            throw new NotFoundException("Not found!");
        }

        return urls;
    }
}