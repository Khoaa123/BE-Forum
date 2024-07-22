namespace Be_Voz_Clone.src.Services
{
    public interface IEmojiAndStickerService
    {
        Task<List<string>> GetUrl(string name);
    }
}
