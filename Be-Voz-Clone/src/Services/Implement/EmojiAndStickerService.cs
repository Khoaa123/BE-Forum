
using Be_Voz_Clone.src.Shared.Core.Exceptions;
using Be_Voz_Clone.src.Shared.Database.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Be_Voz_Clone.src.Services.Implement
{
    public class EmojiAndStickerService : IEmojiAndStickerService
    {
        private readonly AppDbContext _context;

        public EmojiAndStickerService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<string>> GetUrl(string name)
        {
            var urls = await _context.EmojiAndStickers
                .Where(x => x.Name == name)
                .Select(x => x.Url)
                .ToListAsync();

            if (urls == null || !urls.Any())
            {
                throw new NotFoundException("Not found!");
            }


            return urls;
        }
    }
}
