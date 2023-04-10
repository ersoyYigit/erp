using ArdaManager.Application.Extensions;
using ArdaManager.Application.Interfaces.Repositories;
using ArdaManager.Domain.Entities.Catalog;
using ArdaManager.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ArdaManager.Application.Features.Products.Queries.GetProductImage
{
    public class GetProductImageQuery : IRequest<Result<string>>
    {
        public int Id { get; set; }

        public GetProductImageQuery(int productId)
        {
            Id = productId;
        }
    }

    internal class GetProductImageQueryHandler : IRequestHandler<GetProductImageQuery, Result<string>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private string readPath;
        public GetProductImageQueryHandler(IUnitOfWork<int> unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            readPath = configuration["Storage:ArdaFtp:FileReadPath"];
        }

        public async Task<Result<string>> Handle(GetProductImageQuery request, CancellationToken cancellationToken)
        {

            var data = await _unitOfWork.Repository<Product>().Entities.Where(p => p.Id == request.Id).Select(a => a.ImageDataURL).FirstOrDefaultAsync(cancellationToken);
            data = data.ToRelativePath();

            return await Result<string>.SuccessAsync(data: data);
        }
    }
}