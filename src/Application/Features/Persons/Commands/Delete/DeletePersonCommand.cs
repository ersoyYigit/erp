using ArdaManager.Application.Interfaces.Repositories;
using ArdaManager.Domain.Entities.Human;
using ArdaManager.Shared.Wrapper;
using MediatR;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ArdaManager.Application.Features.Persons.Commands.Delete
{
    public class DeletePersonCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class DeletePersonCommandHandler : IRequestHandler<DeletePersonCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<DeletePersonCommandHandler> _localizer;

        public DeletePersonCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeletePersonCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
        }

        public async Task<Result<int>> Handle(DeletePersonCommand command, CancellationToken cancellationToken)
        {
            var person = await _unitOfWork.Repository<Person>().GetByIdAsync(command.Id);
            if (person != null)
            {
                await _unitOfWork.Repository<Person>().DeleteAsync(person);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(person.Id, _localizer["Person Deleted"]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Person Not Found!"]);
            }
        }
    }
}
