using AutoMapper;
using Be_Voz_Clone.src.Model.Entities;
using Be_Voz_Clone.src.Services.DTO.Account;
using Be_Voz_Clone.src.Services.DTO.Category;
using Be_Voz_Clone.src.Services.DTO.Forum;
using Be_Voz_Clone.src.Services.DTO.Thread;

namespace Be_Voz_Clone.src.Services.MappingProfile
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<ApplicationUser, AccountRegisterRequest>().ReverseMap();
            CreateMap<ApplicationUser, AccountRegisterResponse>().ReverseMap();

            CreateMap<ApplicationUser, AccountLoginRequest>().ReverseMap();

            CreateMap<Category, CategoryRequest>().ReverseMap();
            CreateMap<Category, CategoryResponse>();

            CreateMap<Forum, ForumRequest>().ReverseMap();
            CreateMap<Forum, ForumResponse>()
                .ForMember(x => x.CategoryName, opt => opt.MapFrom(src => src.Category.Name));

            CreateMap<VozThread, ThreadRequest>().ReverseMap();
            CreateMap<VozThread, ThreadResponse>();
        }
    }
}
