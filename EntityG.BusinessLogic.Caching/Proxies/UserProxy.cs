using EasyCaching.Core;
using EntityG.BusinessLogic.Caching.Constants;
using EntityG.BusinessLogic.Caching.Interfaces.Proxies;
using EntityG.BusinessLogic.Interfaces.Services.Identity;
using EntityG.Contracts.Responses.Shared;
using EntityG.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EntityG.BusinessLogic.Caching.Proxies
{
    public class UserProxy : IUserProxy
    {
        private readonly IUserService _userService;
        private readonly IEasyCachingProviderFactory _factory;
        private readonly IEasyCachingProvider _provider;

        public UserProxy(IUserService userService, IEasyCachingProviderFactory factory)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
            _provider = factory.GetCachingProvider(nameof(EntityG));
        }

        public async Task<IResult<List<LookupDto>>> GetAllAsync()
        {
            var cachingData = _provider.Get<IResult<List<LookupDto>>>(CachingKey.USERS_LOOKUP);

            if (cachingData.HasValue)
            {
                return cachingData.Value;
            }

            IResult<List<LookupDto>> users = await _userService.GetAllAsync();
            _provider.Set(CachingKey.EMPLOYEES_LOOKUP, users, TimeSpan.FromSeconds(30));

            return users;
        }
    }
}
