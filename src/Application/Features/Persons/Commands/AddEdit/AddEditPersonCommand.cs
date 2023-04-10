using ArdaManager.Application.Interfaces.Repositories;
using ArdaManager.Application.Interfaces.Services;
using ArdaManager.Application.Requests;
using ArdaManager.Domain.Entities.Human;
using ArdaManager.Domain.Enums;
using ArdaManager.Shared.Wrapper;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ArdaManager.Application.Features.Persons.Commands.AddEdit
{
    public class AddEditPersonCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
        
        public string MiddleName { get; set; }
        
        public string SurName { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string Telephone { get; set; }
        public string TelephoneExt { get; set; }
        public string Fax { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string EmailSecondary { get; set; }
        public Gender Gender { get; set; }
        public string Status { get; set; }
        public string BCard { get; set; }
        public string Note { get; set; }
        public string ImageDataURL { get; set; }
        

        public int? CategoryId { get; set; }
        public UploadRequest UploadRequest { get; set; }
    }

    internal class AddEditPersonCommandHandler : IRequestHandler<AddEditPersonCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IUploadService _uploadService;
        private readonly IStringLocalizer<AddEditPersonCommandHandler> _localizer;

        public AddEditPersonCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IUploadService uploadService, IStringLocalizer<AddEditPersonCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _uploadService = uploadService;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AddEditPersonCommand command, CancellationToken cancellationToken)
        {
            if (await _unitOfWork.Repository<Person>().Entities.Where(p => p.Id != command.Id)
                .AnyAsync(p => p.Code == command.Code, cancellationToken))
            {
                return await Result<int>.FailAsync(_localizer["Code already exists."]);
            }

            var uploadRequest = command.UploadRequest;
            if (uploadRequest != null && uploadRequest.Data != null)
            {
                uploadRequest.FileName = $"Person-{command.Code}{uploadRequest.Extension}";
            }

            if (command.Id == 0)
            {
                var person = _mapper.Map<Person>(command);
                if (uploadRequest != null)
                {
                    person.ImageDataURL = await _uploadService.UploadAsync(uploadRequest);
                }
                await _unitOfWork.Repository<Person>().AddAsync(person);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(person.Id, _localizer["Person Saved"]);
            }
            else
            {
                var person = await _unitOfWork.Repository<Person>().GetByIdAsync(command.Id);
                if (person != null)
                {
                    person.Code = command.Code ?? person.Code;
                    person.Name = command.Name ?? person.Name;
                    person.Surname = command.SurName ?? person.Surname;
                    person.MiddleName = command.MiddleName ?? person.MiddleName;
                    person.Title = command.Title ?? person.Title;
                    person.Description = command.Description ?? person.Description;
                    person.Telephone = command.Telephone ?? person.Telephone;
                    person.TelephoneExt = command.TelephoneExt ?? person.TelephoneExt;
                    person.Fax = command.Fax ?? person.Fax;
                    person.Mobile = command.Mobile ?? person.Mobile;
                    person.Email = command.Email ?? person.Email;
                    person.EmailSecondary = command.EmailSecondary ?? person.EmailSecondary;
                    person.Gender = (command.Gender == 0) ? person.Gender : command.Gender;
                    person.Status = command.Status ?? person.Status;
                    person.BCard = command.BCard ?? person.BCard;
                    person.Note = command.Note ?? person.Note;
                    if (uploadRequest != null && uploadRequest.Data != null)
                    {
                        person.ImageDataURL = await _uploadService.UploadAsync(uploadRequest);
                    }
                    else
                        person.ImageDataURL = "";

                    person.CategoryId = command.CategoryId;


                    await _unitOfWork.Repository<Person>().UpdateAsync(person);
                    await _unitOfWork.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(person.Id, _localizer["Person Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Person Not Found!"]);
                }
            }
        }
    }
}
