using ArdaManager.Application.Interfaces.Repositories;
using ArdaManager.Application.Interfaces.Services;
using ArdaManager.Domain.Entities.Corporation;
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

namespace ArdaManager.Application.Features.Companies.Commands.AddEdit
{
    public partial class AddEditCompanyCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public CompanyType CompanyType { get; set; }
        public string Address { get; set; }
        public string WebSite { get; set; }
        public string Telephone { get; set; }
        public string Telephone2 { get; set; }
        public string Fax { get; set; }
        public string Vat { get; set; }
        public string Notes { get; set; }
        
        public int CityId { get; set; }
        public int CountryId { get; set; }
        public int? RepresantativeId { get; set; }

    }

    internal class AddEditCompanyCommandHandler : IRequestHandler<AddEditCompanyCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IUploadService _uploadService;
        private readonly IStringLocalizer<AddEditCompanyCommandHandler> _localizer;

        public AddEditCompanyCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IUploadService uploadService, IStringLocalizer<AddEditCompanyCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _uploadService = uploadService;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AddEditCompanyCommand command, CancellationToken cancellationToken)
        {
            if (await _unitOfWork.Repository<Company>().Entities.Where(p => p.Id != command.Id)
                .AnyAsync(p => p.Code == command.Code, cancellationToken))
            {
                return await Result<int>.FailAsync(_localizer["Code already exists."]);
            }
            /*
            var uploadRequest = command.UploadRequest;
            if (uploadRequest != null)
            {
                uploadRequest.FileName = $"P-{command.Code}{uploadRequest.Extension}";
            }*/

            if (command.Id == 0)
            {
                var company = _mapper.Map<Company>(command);
                /*
                if (uploadRequest != null)
                {
                    product.ImageDataURL = _uploadService.UploadAsync(uploadRequest);
                }*/
                await _unitOfWork.Repository<Company>().AddAsync(company);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(company.Id, _localizer["Company Saved"]);
            }
            else
            {
                var company = await _unitOfWork.Repository<Company>().GetByIdAsync(command.Id);
                if (company != null)
                {
                    company.Name = command.Name ?? company.Name;
                    company.Code = command.Code ?? company.Code;
                    company.Address = command.Address ?? company.Address;
                    company.WebSite = command.WebSite ?? company.WebSite;
                    company.Telephone = command.Telephone?? company.Telephone;
                    company.Telephone2 = command.Telephone2 ?? company.Telephone2;
                    company.Fax = command.Fax?? company.Fax;
                    company.Vat = command.Vat ?? company.Vat;
                    company.Notes = command.Notes ?? company.Notes;
                    company.CountryId = (command.CountryId == 0) ? company.CountryId : command.CountryId;
                    company.CityId = (command.CityId == 0) ? company.CityId : command.CityId;
                    company.Type = (command.CompanyType == 0) ? company.Type : command.CompanyType;
                    company.RepresantativeId = (command.RepresantativeId == 0) ? company.RepresantativeId : command.RepresantativeId;
                    company.RepresantativeId = (command.RepresantativeId == 0) ? company.RepresantativeId : command.RepresantativeId;
                    /*
                    if (uploadRequest != null)
                    {
                        product.ImageDataURL = _uploadService.UploadAsync(uploadRequest);
                    }*/

                    await _unitOfWork.Repository<Company>().UpdateAsync(company);
                    await _unitOfWork.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(company.Id, _localizer["Company Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Company Not Found!"]);
                }
            }
        }
    }
}
