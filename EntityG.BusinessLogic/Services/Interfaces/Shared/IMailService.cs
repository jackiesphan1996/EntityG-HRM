using EntityG.Contracts.Requests.Shared;
using System.Threading.Tasks;

namespace EntityG.BusinessLogic.Services.Interfaces.Shared
{
    public interface IMailService
    {
        Task SendAsync(MailRequest request);
    }
}