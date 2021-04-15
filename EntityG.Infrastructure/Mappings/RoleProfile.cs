using AutoMapper;
using EntityG.Application.Responses.Identity;
using Microsoft.AspNetCore.Identity;

namespace EntityG.Infrastructure.Mappings
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<RoleResponse, IdentityRole>().ReverseMap();
        }
    }
}