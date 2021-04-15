using EntityG.Client.Infrastructure.Extensions;
using EntityG.Client.Infrastructure.Routes;
using EntityG.Shared.Wrapper;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using EntityG.Contracts.Responses.Leaves;

namespace EntityG.Client.Infrastructure.Managers.Leaves
{
    public class LeaveManager : ILeaveManager
    {
        private readonly HttpClient _httpClient;

        public LeaveManager(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<PagingResult<LeaveDto>> GetAllAsync(int page, int pageSize, DateTime fromDate, DateTime toDate, bool? isApproved)
        {
            var response = await _httpClient.GetAsync(LeaveEndPoint.GetAll(page, pageSize, fromDate, toDate, isApproved));

            return await response.ToPagingResult<LeaveDto>();
        }
    }
}
