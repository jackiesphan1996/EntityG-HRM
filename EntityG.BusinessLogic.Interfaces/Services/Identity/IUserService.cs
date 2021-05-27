using EntityG.Contracts.Requests.Identity;
using EntityG.Contracts.Responses.Identity;
using EntityG.Contracts.Responses.Shared;
using EntityG.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EntityG.BusinessLogic.Interfaces.Services.Identity
{
    public interface IUserService
    {
        Task<PagingResult<UserResponse>> GetAllAsync(int page, int pageSize, string keySearch);
        Task<int> GetCountAsync();
        Task<IResult<UserResponse>> GetAsync(string userId);
        Task<IResult> RegisterAsync(RegisterRequest request, string origin);
        Task<IResult> ToggleUserStatusAsync(ToggleUserStatusRequest request);
        Task<IResult<UserRolesResponse>> GetRolesAsync(string id);
        Task<IResult> UpdateRolesAsync(UpdateUserRolesRequest request);
        Task<IResult<string>> ConfirmEmailAsync(string userId, string code);
        Task<IResult> ForgotPasswordAsync(string emailId, string origin);
        Task<IResult> ResetPasswordAsync(ResetPasswordRequest request);
        Task<IResult<List<LookupDto>>> GetAllAsync();
    }
}