using EntityG.Contracts.Requests.Identity;
using EntityG.Shared.Wrapper;
using System.Threading.Tasks;

namespace EntityG.BusinessLogic.Interfaces.Services.Account
{
    public interface IAccountService
    {
        Task<IResult> UpdateProfileAsync(UpdateProfileRequest model, string userId);

        Task<IResult> ChangePasswordAsync(ChangePasswordRequest model, string userId);

        Task<IResult<string>> GetProfilePictureAsync(string userId);

        Task<IResult> UpdateProfilePictureAsync(UpdateProfilePictureRequest request, string userId);
    }
}