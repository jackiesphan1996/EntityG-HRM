using EntityG.BusinessLogic.Interfaces.Services.Shared;
using EntityG.Contracts.Requests.Shared;
using System.Threading.Tasks;

namespace EntityG.BusinessLogic.Services.Shared
{
    public class MailService : IMailService
    {
        public Task SendAsync(MailRequest request)
        {
            throw new System.NotImplementedException();
        }
    }
}
