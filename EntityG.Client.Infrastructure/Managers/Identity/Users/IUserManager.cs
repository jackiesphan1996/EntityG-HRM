using System.Collections.Generic;
using EntityG.Shared.Wrapper;
using System.Threading.Tasks;
using EntityG.Contracts.Requests.Identity;
using EntityG.Contracts.Responses.Identity;
using EntityG.Contracts.Responses.Shared;

namespace EntityG.Client.Infrastructure.Managers.Identity.Users
{
    public interface IUserManager : IManager
    {
        Task<PagingResult<UserResponse>> GetAllAsync(int page, int pageIndex, string search);
        Task<IResult> ForgotPasswordAsync(ForgotPasswordRequest request);
        Task<IResult> ResetPasswordAsync(ResetPasswordRequest request);
        Task<IResult<UserResponse>> GetAsync(string userId);
        Task<IResult<UserRolesResponse>> GetRolesAsync(string userId);
        Task<IResult> RegisterUserAsync(RegisterRequest request);
        Task<IResult> ToggleUserStatusAsync(ToggleUserStatusRequest request);
        Task<IResult> UpdateRolesAsync(UpdateUserRolesRequest request);
        Task<IResult<List<LookupDto>>> GetAllAsync();
    }
}