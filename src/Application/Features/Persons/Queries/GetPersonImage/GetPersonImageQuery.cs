using ArdaManager.Application.Interfaces.Repositories;
using ArdaManager.Domain.Entities.Human;
using ArdaManager.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ArdaManager.Application.Features.Persons.Queries.GetPersonImage
{
    public class GetPersonImageQuery : IRequest<Result<string>>
    {
        public int Id { get; set; }

        public GetPersonImageQuery(int personId)
        {
            Id = personId;
        }
    }

    internal class GetProductImageQueryHandler : IRequestHandler<GetPersonImageQuery, Result<string>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public GetProductImageQueryHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<string>> Handle(GetPersonImageQuery request, CancellationToken cancellationToken)
        {
            var data = await _unitOfWork.Repository<Person>().Entities.Where(p => p.Id == request.Id).Select(a => a.ImageDataURL).FirstOrDefaultAsync(cancellationToken);
            return await Result<string>.SuccessAsync(data: data);
        }
    }
}
