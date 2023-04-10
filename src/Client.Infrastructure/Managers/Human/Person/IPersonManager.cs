using ArdaManager.Application.Features.Persons.Commands.AddEdit;
using ArdaManager.Application.Features.Persons.Queries.GetAllPaged;
using ArdaManager.Application.Requests.Human;
using ArdaManager.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Client.Infrastructure.Managers.Human.Person
{
    public interface IPersonManager : IManager
    {
        Task<PaginatedResult<GetAllPagedPersonsResponse>> GetPersonsAsync(GetAllPagedPersonsRequest request);

        Task<IResult<string>> GetPersonImageAsync(int id);

        Task<IResult<int>> SaveAsync(AddEditPersonCommand request);

        Task<IResult<int>> DeleteAsync(int id);

        Task<IResult<string>> ExportToExcelAsync(string searchString = "");
    }
}
