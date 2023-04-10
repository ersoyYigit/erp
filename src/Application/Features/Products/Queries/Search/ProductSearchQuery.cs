using ArdaManager.Application.Features.Products.Queries.Search;
using ArdaManager.Application.Interfaces.Repositories;
using ArdaManager.Domain.Entities.Catalog;
using ArdaManager.Domain.Enums;
using ArdaManager.Shared.Wrapper;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

public class ProductSearchQuery : IRequest<Result<List<ProductSearchResultDto>>>
{
    public ProductType Type { get; set; } = ProductType.Raw;
    public string SearchTerm { get; set; }
}

public class ProductSearchQueryHandler : IRequestHandler<ProductSearchQuery, Result<List<ProductSearchResultDto>>>
{
    private readonly IUnitOfWork<int> _unitOfWork;

    public ProductSearchQueryHandler(IUnitOfWork<int> unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<List<ProductSearchResultDto>>> Handle(ProductSearchQuery request, CancellationToken cancellationToken)
    {
        var searchTerm = request.SearchTerm;
        /*

        Expression<Func<Product, bool>> 
            filter = p => ((string.IsNullOrEmpty(searchTerm) || p.Name.Contains(searchTerm) || p.Code.Contains(searchTerm) )
                            && p.Type == ArdaManager.Domain.Enums.ProductType.Raw);

        var products = await _unitOfWork.Repository<Product>()
            .GetAllAsync(
                predicate: filter,
                include: p => p.Include(x => x.Category)
                              .Include(x => x.MeasurementUnit),
                take: 25
            );

        var result = products.Select(p => new ProductSearchResultDto
        {
            ProductId = p.Id,
            Name = p.Name,
            Code = p.Code,
            MeasurementUnitName = p.MeasurementUnit.Name,
            MeasurementUnitSystem = p.MeasurementUnit.System,
            MeasurementUnitId = p.MeasurementUnit.Id,
            CategoryId = p.Category?.Id,
            CategoryName = p.Category?.Name
        }).ToList();

        return await Result<List<ProductSearchResultDto>>.SuccessAsync(result);
         */
        var parameters = new List<SqlParameter>{
            new SqlParameter("@Type", SqlDbType.Int) { Value = 0 },
        };

        if (!string.IsNullOrEmpty(searchTerm))
        {
            parameters.Add(new SqlParameter("@SearchStr", SqlDbType.NVarChar) { Value = searchTerm });
        }

        var query = "sp_search_products_with_stocks";

        var result = await _unitOfWork.VappRepository.ExecuteStoredProcedureAsync<ProductSearchResultDto>(query, parameters.ToArray());

        return Result<List<ProductSearchResultDto>>.Success(result.ToList());
    }
}