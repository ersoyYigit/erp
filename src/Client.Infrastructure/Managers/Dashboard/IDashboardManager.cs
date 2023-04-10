using ArdaManager.Shared.Wrapper;
using System.Threading.Tasks;
using ArdaManager.Application.Features.Dashboards.Queries.GetData;

namespace ArdaManager.Client.Infrastructure.Managers.Dashboard
{
    public interface IDashboardManager : IManager
    {
        Task<IResult<DashboardDataResponse>> GetDataAsync();
    }
}