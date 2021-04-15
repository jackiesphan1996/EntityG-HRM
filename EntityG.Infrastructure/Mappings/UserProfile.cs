using AutoMapper;
using EntityG.Application.Responses.Identity;
using EntityG.Domain.Entities.Identity;

namespace EntityG.Infrastructure.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserResponse, ApplicationUser>().ReverseMap();
        }
    }
}