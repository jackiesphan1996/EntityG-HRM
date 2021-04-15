using EntityG.Contracts.Responses.Shared;
using EntityG.Client.Infrastructure.Extensions;
using EntityG.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace EntityG.Client.Infrastructure.Managers.LeaveType
{
    public class LeaveTypeManager : ILeaveTypeManager
    {
        private readonly HttpClient _httpClient;

        public LeaveTypeManager(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<IResult<List<LookupDto>>> GetAllAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync(Routes.LeaveTypeEndpoint.GetAll);
            return await response.ToResult<List<LookupDto>>();
        }
    }
}
