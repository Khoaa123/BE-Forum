using Be_Voz_Clone.src.Shared.Core.Enums;

namespace Be_Voz_Clone.src.Model.Entities;

public class Reaction
{
    public int Id { get; set; }
    public ReactionType Type { get; set; }
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
    public int CommentId { get; set; }
    public Comment Comment { get; set; }
    public DateTime CreatedAt { get; set; }
}