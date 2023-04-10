using ArdaManager.Application.Features.Docs.TemplateWorkOrders.Commands.AddEdit;
using ArdaManager.Application.Features.Docs.TemplateWorkOrders.Queries.GetAll;
using ArdaManager.Application.Features.Docs.TemplateWorkOrders.Queries.GetById;
using ArdaManager.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Client.Infrastructure.Managers.Docs.TemplateWorkOrder
{
    public interface ITemplateWorkOrderManager : IManager
    {
        Task<IResult<List<GetAllTemplateWorkOrdersResponse>>> GetAllAsync();

        Task<IResult<GetTemplateWorkOrderByIdResponse>> GetByIdAsync(GetTemplateWorkOrderByIdQuery query);

        Task<IResult<int>> SaveAsync(AddEditTemplateWorkOrderCommand request);

        Task<IResult<int>> DeleteAsync(int id);



        
    }
}
