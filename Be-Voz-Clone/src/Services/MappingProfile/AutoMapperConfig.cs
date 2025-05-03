using AutoMapper;
using Be_Voz_Clone.src.Model.Entities;
using Be_Voz_Clone.src.Services.DTO.Account;
using Be_Voz_Clone.src.Services.DTO.Category;
using Be_Voz_Clone.src.Services.DTO.Comment;
using Be_Voz_Clone.src.Services.DTO.Forum;
using Be_Voz_Clone.src.Services.DTO.Reaction;
using Be_Voz_Clone.src.Services.DTO.SavedThread;
using Be_Voz_Clone.src.Services.DTO.Search;
using Be_Voz_Clone.src.Services.DTO.Thread;
using Be_Voz_Clone.src.Services.DTO.User;
using Be_Voz_Clone.src.Services.DTO.ViewedThread;

namespace Be_Voz_Clone.src.Services.MappingProfile;

public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        CreateMap<ApplicationUser, AccountRegisterRequest>().ReverseMap();
        CreateMap<ApplicationUser, AccountRegisterResponse>().ReverseMap();
        CreateMap<ApplicationUser, AccountLoginRequest>().ReverseMap();
        CreateMap<ApplicationUser, UserResponse>().ReverseMap();
        CreateMap<ApplicationUser, AccountResponse>();

        CreateMap<Category, CategoryRequest>().ReverseMap();
        CreateMap<Category, CategoryResponse>()
            .ForMember(x => x.ForumCount, opt => opt.MapFrom(src => src.Forums.Count));
        CreateMap<Forum, ForumRequest>().ReverseMap();
        CreateMap<Forum, ForumResponse>()
            .ForMember(x => x.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
            .ForMember(x => x.ThreadCount, opt => opt.MapFrom(src => src.Threads.Count))
            .ForMember(x => x.LatestThread, opt => opt.MapFrom(src => src.Threads
                .OrderByDescending(t => t.CreatedAt)
                .Select(t => new LatestThreadResponse
                {
                    Id = t.Id,
                    Title = t.Title,
                    CreatedAt = t.CreatedAt,
                    AvatarUrl = t.User.AvatarUrl,
                    DisplayName = t.User.DisplayName,
                    UserId = t.UserId
                }).FirstOrDefault()))
            .ForMember(x => x.TotalComments, opt => opt.MapFrom(src => src.Threads.Sum(t => t.Comments.Count)))
            .ReverseMap();
        CreateMap<VozThread, ThreadRequest>().ReverseMap();
        CreateMap<VozThread, ThreadResponse>()
            .ForMember(x => x.DisplayName, opt => opt.MapFrom(src => src.User.DisplayName))
            .ForMember(x => x.AvatarUrl, opt => opt.MapFrom(src => src.User.AvatarUrl))
            .ForMember(x => x.ForumName, opt => opt.MapFrom(src => src.Forum.Name))
            .ForMember(x => x.TotalComments, opt => opt.MapFrom(src => src.Comments.Count))
            .ForMember(x => x.LastCommentAt,
                opt => opt.MapFrom(src =>
                    src.Comments.Any()
                        ? src.Comments.OrderByDescending(c => c.CreatedAt).First().CreatedAt
                        : src.CreatedAt))
            .ForMember(x => x.LastCommentBy,
                opt => opt.MapFrom(src =>
                    src.Comments.Any()
                        ? src.Comments.OrderByDescending(c => c.CreatedAt).First().User.DisplayName
                        : src.User.DisplayName))
            .ForMember(x => x.LastCommenterAvatarUrl,
                opt => opt.MapFrom(src =>
                    src.Comments.Any()
                        ? src.Comments.OrderByDescending(c => c.CreatedAt).First().User.AvatarUrl
                        : src.User.AvatarUrl))
            .ReverseMap();
        CreateMap<VozThread, LatestThreadResponse>()
            .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.User.DisplayName))
            .ForMember(dest => dest.AvatarUrl, opt => opt.MapFrom(src => src.User.AvatarUrl));
        CreateMap<VozThread, SearchResponse>()
            .ForMember(x => x.UserName, opt => opt.MapFrom(src => src.User.DisplayName))
            .ForMember(x => x.AvatarUrl, opt => opt.MapFrom(src => src.User.AvatarUrl))
            .ForMember(x => x.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(x => x.ForumName, opt => opt.MapFrom(src => src.Forum.Name))
            .ForMember(x => x.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
            .ReverseMap();
        CreateMap<ViewedThread, ViewedThreadResponse>()
            .ForMember(x => x.DisplayName, opt => opt.MapFrom(src => src.User.DisplayName))
            .ForMember(x => x.Tag, opt => opt.MapFrom(src => src.Thread.Tag))
            .ForMember(x => x.ForumName, opt => opt.MapFrom(src => src.Thread.Forum.Name))
            .ForMember(x => x.CreatedAt, opt => opt.MapFrom(src => src.Thread.CreatedAt))
            .ForMember(x => x.ThreadName, opt => opt.MapFrom(src => src.Thread.Title))
            .ForMember(x => x.ThreadContent, opt => opt.MapFrom(src => src.Thread.Content))
            .ReverseMap();
        CreateMap<Comment, CommentRequest>().ReverseMap();
        CreateMap<Comment, CommentResponse>()
            .ForMember(x => x.ThreadName, opt => opt.MapFrom(src => src.Thread.Title))
            .ForMember(x => x.UserName, opt => opt.MapFrom(src => src.User.DisplayName))
            .ForMember(x => x.AvatarUrl, opt => opt.MapFrom(src => src.User.AvatarUrl))
            .ReverseMap();
        CreateMap<Reaction, ReactionRequest>().ReverseMap();
        CreateMap<Reaction, ReactionResponse>()
            .ForMember(x => x.UserName, opt => opt.MapFrom(src => src.User.DisplayName))
            .ForMember(x => x.AvatarUrl, opt => opt.MapFrom(src => src.User.AvatarUrl))
            .ReverseMap();
        CreateMap<SavedThread, SavedThreadResponse>()
            .ForMember(x => x.DisplayName, opt => opt.MapFrom(src => src.Thread.User.DisplayName))
            .ForMember(x => x.Tag, opt => opt.MapFrom(src => src.Thread.Tag))
            .ForMember(x => x.ForumName, opt => opt.MapFrom(src => src.Thread.Forum.Name))
            .ForMember(x => x.CreatedAt, opt => opt.MapFrom(src => src.Thread.CreatedAt))
            .ForMember(x => x.ThreadName, opt => opt.MapFrom(src => src.Thread.Title))
            .ForMember(x => x.ThreadContent, opt => opt.MapFrom(src => src.Thread.Content))
            .ReverseMap();
    }
}