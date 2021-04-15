using EntityG.Application.Interfaces.Services;
using System;

namespace EntityG.Infrastructure.Shared.Services
{
    public class SystemDateTimeService : IDateTimeService
    {
        public DateTime NowUtc => DateTime.UtcNow;
    }
}