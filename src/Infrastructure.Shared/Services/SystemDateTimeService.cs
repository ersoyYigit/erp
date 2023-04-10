using ArdaManager.Application.Interfaces.Services;
using System;

namespace ArdaManager.Infrastructure.Shared.Services
{
    public class SystemDateTimeService : IDateTimeService
    {
        public DateTime NowUtc => DateTime.UtcNow;
    }
}