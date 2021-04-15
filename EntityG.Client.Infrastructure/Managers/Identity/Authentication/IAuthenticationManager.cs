using EntityG.Contracts.Requests.Identity;
using EntityG.Shared.Wrapper;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EntityG.Client.Infrastructure.Managers.Identity.Authentication
{
    public interface IAuthenticationManager : IManager
    {
        Task<IResult> Login(TokenRequest model);

        Task<IResult> Logout();
        Task<string> RefreshToken();
        Task<string> TryRefreshToken();

        Task<ClaimsPrincipal> CurrentUser();
    }
}