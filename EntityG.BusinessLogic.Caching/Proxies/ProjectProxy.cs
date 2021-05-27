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
    public class ProjectProxy : IProjectProxy
    {
        private readonly IProjectService _projectService;
        private readonly IEasyCachingProviderFactory _factory;
        private readonly IEasyCachingProvider _provider;

        public ProjectProxy(IProjectService projectService, IEasyCachingProviderFactory factory)
        {
            _projectService = projectService ?? throw new ArgumentNullException(nameof(projectService));
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
            _provider = factory.GetCachingProvider(nameof(EntityG));
        }

        public async Task<IResult<List<LookupDto>>> GetAllAsync()
        {
            var cachingData = _provider.Get<IResult<List<LookupDto>>>(CachingKey.PROJECTS_LOOKUP);

            if (cachingData.HasValue)
            {
                return cachingData.Value;
            }

            IResult<List<LookupDto>> users = await _projectService.GetAllAsync();
            _provider.Set(CachingKey.PROJECTS_LOOKUP, users, TimeSpan.FromHours(5));

            return users;
        }
    }
}
