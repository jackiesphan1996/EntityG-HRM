using EntityG.Contracts.Requests.Identity;
using EntityG.Shared.Wrapper;
using System.Threading.Tasks;

namespace EntityG.Client.Infrastructure.Managers.Identity.Account
{
    public interface IAccountManager : IManager
    {
        Task<IResult> ChangePasswordAsync(ChangePasswordRequest model);

        Task<IResult> UpdateProfileAsync(UpdateProfileRequest model);

        Task<IResult<string>> GetProfilePictureAsync(string userId);

        Task<IResult> UpdateProfilePictureAsync(UpdateProfilePictureRequest request, string userId);
    }
}