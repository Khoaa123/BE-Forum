using Be_Voz_Clone.src.Model.Entities;
using Be_Voz_Clone.src.Shared.Database.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Be_Voz_Clone.src.Repositories.Implement
{
    public class EmojiAndStickerRepository : BaseRepository<EmojiAndSticker>, IEmojiAndStickerRepository
    {
        private readonly AppDbContext _context;

        public EmojiAndStickerRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<string>> GetUrlsByNameAsync(string name)
        {
            return await _context.EmojiAndStickers
                .Where(e => e.Name == name)
                .Select(e => e.Url)
                .ToListAsync();
        }
    }
}