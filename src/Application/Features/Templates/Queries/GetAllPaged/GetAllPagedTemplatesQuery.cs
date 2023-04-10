using ArdaManager.Application.Extensions;
using ArdaManager.Application.Interfaces.Repositories;
using ArdaManager.Application.Specifications.Catalog;
using ArdaManager.Domain.Entities.Catalog;
using ArdaManager.Shared.Wrapper;
using MediatR;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using ArdaManager.Domain.Enums;
using System.Data;
using Microsoft.Extensions.Logging;
using ArdaManager.Application.Features.Templates.Queries.GetAllPaged;

namespace ArdaManager.Application.Features.Templates.Queries.GetAllPaged
{
    public class GetAllPagedTemplatesQuery : IRequest<PaginatedResult<GetAllPagedTemplatesResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchString { get; set; }
        public string[] OrderBy { get; set; } // of the form fieldname [ascending|descending],fieldname [ascending|descending]...

        public GetAllPagedTemplatesQuery(int pageNumber, int pageSize, string searchString, string orderBy = "")
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            SearchString = searchString;
            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                OrderBy = orderBy.Split(',');
            }
        }
    }

    internal class GetAllTemplatesQueryHandler : IRequestHandler<GetAllPagedTemplatesQuery, PaginatedResult<GetAllPagedTemplatesResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly ILogger<GetAllTemplatesQueryHandler> _logger;

        public GetAllTemplatesQueryHandler(IUnitOfWork<int> unitOfWork, ILogger<GetAllTemplatesQueryHandler> logger)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedResult<GetAllPagedTemplatesResponse>> Handle(GetAllPagedTemplatesQuery request, CancellationToken cancellationToken)
        {
            _logger.Log(LogLevel.Error, "Custom error");
            Expression<Func<Template, GetAllPagedTemplatesResponse>> expression = e => new GetAllPagedTemplatesResponse
            {
                Id = e.Id,
                Name = e.Name,
                Description = e.Description,
                Code = e.Code,
                Pattern = e.Pattern.Name,
                PatternId = e.PatternId ?? 0,
                Category = e.Category.Name,
                CategoryId = e.CategoryId,
                ImageDataURL = e.ImageDataURL.ToRelativePath(),

                Weight = e.Weight,
                WeightTolerance = e.WeightTolerance,
                WeightMUId = e.WeightMUId,

                Diameter = e.Diameter,
                DiameterTolerance = e.DiameterTolerance,
                DiameterMUId = e.DiameterMUId,

                Height = e.Height,
                HeightTolerance = e.HeightTolerance,
                HeightMUId = e.HeightMUId,

                Length = e.Length,
                LengthTolerance = e.LengthTolerance,
                LengthMUId = e.LengthMUId,

                Width = e.Width,
                WidthTolerance = e.WidthTolerance,
                WidthMUId = e.WidthMUId,


                TemplatePatternId = e.PatternId ?? 0,
                ProductionDate = e.ProductionDate,
                TemplateKind= e.TemplateKind,
                State = e.State,
                IsAlabastr = e.IsAlabastr,
                RevisionDate= e.RevisionDate,
                RevisionRequester= e.RevisionRequester,
                RevisionRequesterDepartment= e.RevisionRequesterDepartment,
                FaultDate= e.FaultDate,
                FaultCause= e.FaultCause,
                FixDate = e.FixDate,
                FaultFixer = e.FaultFixer,
                CancelCause= e.CancelCause,
                Canceller   = e.Canceller,
                CancelationDate= e.CancelationDate,




                BoolProperty1 = e.BoolProperty1,
                Type = e.Type,
                Kind = e.Kind,
                TemplateState= e.TemplateState,
                TemplateId = e.TemplateId,
                LastProductionDate = e.LastProductionDate,
                //MeasurementUnit = e.MeasurementUnit.Code,
                MeasurementUnitId = e.MeasurementUnitId,
                String1 = e.string1,
                Guid = e.Guid,
            };
            var productFilterSpec = new TemplateFilterSpecification(request.SearchString);

            if (request.OrderBy?.Any() != true)
            {
                var data = await _unitOfWork.Repository<Template>().Entities
                   .Specify(productFilterSpec)
                   .Select(expression)
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;
            }
            else
            {
                var ordering = string.Join(" ", request.OrderBy); // of the form fieldname [ascending|descending], ...
                var data = await _unitOfWork.Repository<Template>().Entities
                   .Specify(productFilterSpec)
                   .OrderBy(ordering) // require system.linq.dynamic.core
                   .Select(expression)
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;

            }
        }
    }
}