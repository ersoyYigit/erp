using System.Collections.Generic;
using System.Threading.Tasks;
using ArdaManager.Application.Interfaces.Common;
using ArdaManager.Application.Requests.Identity;
using ArdaManager.Application.Responses.Identity;
using ArdaManager.Shared.Wrapper;

namespace ArdaManager.Application.Interfaces.Services.Identity
{
    public interface IRoleClaimService : IService
    {
        Task<Result<List<RoleClaimResponse>>> GetAllAsync();

        Task<int> GetCountAsync();

        Task<Result<RoleClaimResponse>> GetByIdAsync(int id);

        Task<Result<List<RoleClaimResponse>>> GetAllByRoleIdAsync(string roleId);

        Task<Result<string>> SaveAsync(RoleClaimRequest request);

        Task<Result<string>> DeleteAsync(int id);
    }
}