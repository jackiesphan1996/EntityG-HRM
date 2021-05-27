using EntityG.Contracts.Requests.Shared;
using System.Threading.Tasks;

namespace EntityG.BusinessLogic.Interfaces.Services.Shared
{
    public interface IMailService
    {
        Task SendAsync(MailRequest request);
    }
}