using Be_Voz_Clone.src.Model.Entities;

namespace Be_Voz_Clone.src.Repositories
{
    public interface IEmojiAndStickerRepository : IBaseRepository<EmojiAndSticker>
    {
        Task<List<string>> GetUrlsByNameAsync(string name);
    }
}
