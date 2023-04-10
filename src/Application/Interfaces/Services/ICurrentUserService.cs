using ArdaManager.Application.Interfaces.Common;

namespace ArdaManager.Application.Interfaces.Services
{
    public interface ICurrentUserService : IService
    {
        string UserId { get; }
    }
}