using System.Collections.Generic;
using System.Threading.Tasks;
using ArdaManager.Application.Requests.Identity;
using ArdaManager.Application.Responses.Identity;
using ArdaManager.Shared.Wrapper;

namespace ArdaManager.Client.Infrastructure.Managers.Identity.RoleClaims
{
    public interface IRoleClaimManager : IManager
    {
        Task<IResult<List<RoleClaimResponse>>> GetRoleClaimsAsync();

        Task<IResult<List<RoleClaimResponse>>> GetRoleClaimsByRoleIdAsync(string roleId);

        Task<IResult<string>> SaveAsync(RoleClaimRequest role);

        Task<IResult<string>> DeleteAsync(string id);
    }
}