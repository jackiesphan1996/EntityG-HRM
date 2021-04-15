using AutoMapper;
using EntityG.Contracts.Responses.Identity;
using EntityG.EntityFramework.Entities.Identity;

namespace EntityG.BusinessLogic.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserResponse, ApplicationUser>().ReverseMap();
        }
    }
}