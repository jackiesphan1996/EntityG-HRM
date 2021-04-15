using AutoMapper;
using EntityG.Contracts.Responses.Identity;
using Microsoft.AspNetCore.Identity;

namespace EntityG.BusinessLogic.Mappings
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<RoleResponse, IdentityRole>().ReverseMap();
        }
    }
}