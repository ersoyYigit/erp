using ArdaManager.Application.Interfaces.Common;
using ArdaManager.Application.Requests.Identity;
using ArdaManager.Application.Responses.Identity;
using ArdaManager.Shared.Wrapper;
using System.Threading.Tasks;

namespace ArdaManager.Application.Interfaces.Services.Identity
{
    public interface ITokenService : IService
    {
        Task<Result<TokenResponse>> LoginAsync(TokenRequest model);

        Task<Result<TokenResponse>> GetRefreshTokenAsync(RefreshTokenRequest model);
    }
}