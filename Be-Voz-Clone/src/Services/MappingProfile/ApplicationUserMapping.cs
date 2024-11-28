using AutoMapper;
using Be_Voz_Clone.src.Model.Entities;
using Be_Voz_Clone.src.Services.DTO.Account;
using Be_Voz_Clone.src.Services.DTO.User;

namespace Be_Voz_Clone.src.Services.MappingProfile
{
    public class ApplicationUserMapping : Profile
    {
        public ApplicationUserMapping()
        {
            CreateMap<ApplicationUser, AccountRegisterRequest>().ReverseMap();
            CreateMap<ApplicationUser, AccountRegisterResponse>().ReverseMap();
            CreateMap<ApplicationUser, AccountLoginRequest>().ReverseMap();
            CreateMap<ApplicationUser, UserResponse>().ReverseMap();
            CreateMap<ApplicationUser, AccountResponse>();
        }
    }
}
