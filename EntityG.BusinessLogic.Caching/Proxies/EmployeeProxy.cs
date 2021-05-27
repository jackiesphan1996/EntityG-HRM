using EasyCaching.Core;
using EntityG.BusinessLogic.Caching.Constants;
using EntityG.BusinessLogic.Caching.Interfaces.Proxies;
using EntityG.BusinessLogic.Interfaces.Services;
using EntityG.Contracts.Responses.Shared;
using EntityG.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EntityG.BusinessLogic.Caching.Proxies
{
    public class EmployeeProxy : IEmployeeProxy
    {
        private readonly IEmployeeService _employeeService;
        private readonly IEasyCachingProviderFactory _factory;
        private readonly IEasyCachingProvider _provider;

        public EmployeeProxy(IEmployeeService employeeService, IEasyCachingProviderFactory factory)
        {
            _employeeService = employeeService ?? throw new ArgumentNullException(nameof(employeeService));
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
            _provider = factory.GetCachingProvider(nameof(EntityG));
        }

        public async Task<IResult<List<LookupDto>>> GetAllAsync()
        {
            var cachingData = _provider.Get<IResult<List<LookupDto>>>(CachingKey.EMPLOYEES_LOOKUP);

            if (cachingData.HasValue)
            {
                return cachingData.Value;
            }

            IResult<List<LookupDto>> employees  = await _employeeService.GetAllAsync();
            _provider.Set(CachingKey.EMPLOYEES_LOOKUP, employees, TimeSpan.FromSeconds(30));

            return employees;
        }
    }
}
