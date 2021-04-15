using AutoMapper;
using EntityG.Contracts.Requests.Identity;
using EntityG.Contracts.Responses.Identity;

namespace EntityG.Client.Infrastructure.Mappings
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<PermissionResponse, PermissionRequest>().ReverseMap();
            CreateMap<RoleClaimsResponse, RoleClaimsRequest>().ReverseMap();
        }
    }
}