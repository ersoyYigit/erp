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
using Microsoft.Extensions.Configuration;
using System.IO;

namespace ArdaManager.Application.Features.Products.Queries.GetAllPaged
{
    public class GetAllProductsQuery : IRequest<PaginatedResult<GetAllPagedProductsResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchString { get; set; }
        public string[] OrderBy { get; set; } // of the form fieldname [ascending|descending],fieldname [ascending|descending]...

        public GetAllProductsQuery(int pageNumber, int pageSize, string searchString, string orderBy = "")
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

    internal class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, PaginatedResult<GetAllPagedProductsResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly ILogger<GetAllProductsQueryHandler> _logger;
        public GetAllProductsQueryHandler(IUnitOfWork<int> unitOfWork, ILogger<GetAllProductsQueryHandler> logger)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            
        }

        public async Task<PaginatedResult<GetAllPagedProductsResponse>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            _logger.Log(LogLevel.Error, "Custom error");
            Expression<Func<Product, GetAllPagedProductsResponse>> expression = e => new GetAllPagedProductsResponse
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
                //ImageDataURL = e.ImageDataURL,

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



                BoolProperty1 = e.BoolProperty1,
                Type = e.Type,
                Kind = e.Kind,
                TemplateState= e.TemplateState,
                TemplateId = e.TemplateId,
                LastProductionDate = e.LastProductionDate,
                //MeasurementUnit = e.MeasurementUnit.Code,
                MeasurementUnitId = e.MeasurementUnitId,
                String1 = e.string1
            };
            var productFilterSpec = new ProductFilterSpecification(request.SearchString);

            if (request.OrderBy?.Any() != true)
            {
                var data = await _unitOfWork.Repository<Product>().Entities
                   .Specify(productFilterSpec)
                   .Select(expression)
                   .Where(x=> x.Type != ProductType.Template)
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;
            }
            else
            {
                var ordering = string.Join(" ", request.OrderBy); // of the form fieldname [ascending|descending], ...
                var data = await _unitOfWork.Repository<Product>().Entities
                   .Specify(productFilterSpec)
                   .OrderBy(ordering) // require system.linq.dynamic.core
                   .Select(expression)
                   .Where(x => x.Type != ProductType.Template)
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;

            }
        }
    }
}