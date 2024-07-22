using Microsoft.AspNetCore.Identity;

namespace Be_Voz_Clone.src.Model.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string DisplayName { get; set; } // Tên trong diễn đàn
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        public DateTime JoinedDate { get; set; }
        public string? AvatarUrl { get; set; }
        public string ReactionScore { get; set; }
        public ICollection<VozThread> Threads { get; set; }
    }
}
