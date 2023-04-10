using System.ComponentModel.DataAnnotations;
using AutoMapper;
using ArdaManager.Application.Interfaces.Repositories;
using ArdaManager.Application.Interfaces.Services;
using ArdaManager.Application.Requests;
using ArdaManager.Domain.Entities.Catalog;
using ArdaManager.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using ArdaManager.Domain.Enums;
using static ArdaManager.Shared.Constants.Permission.Permissions;

namespace ArdaManager.Application.Features.Products.Commands.AddEdit
{
    public partial class AddEditProductCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string ImageDataURL { get; set; }
        public decimal Rate { get; set; }
        public int PatternId { get; set; }

        public int MeasurementUnitId { get; set; }


        public decimal Diameter { get; set; }
        public decimal DiameterTolerance { get; set; }
        public int DiameterMUId { get; set; }
        public decimal Weight { get; set; }
        public decimal WeightTolerance { get; set; }
        public int WeightMUId { get; set; }
        public decimal Height { get; set; }
        public decimal HeightTolerance { get; set; }
        public int HeightMUId { get; set; }
        public decimal Length { get; set; }
        public decimal LengthTolerance { get; set; }
        public int LengthMUId { get; set; }
        public decimal Width { get; set; }
        public decimal WidthTolerance { get; set; }
        public int WidthMUId { get;  set; }
        public DateTime? LastProductionDate { get; set; }
        public ProductType Type { get; set; }
        public ProductKind Kind { get; set; }
        public TemplateState TemplateState { get; set; }
        public int? TemplateId { get; set; }
        public bool? BoolProperty1 { get; set; }
        public string String1 { get; set; }


        public Guid Guid { get; set; }
        public int? CategoryId { get; set; }
        public UploadRequest UploadRequest { get; set; }
    }

    internal class AddEditProductCommandHandler : IRequestHandler<AddEditProductCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IUploadService _uploadService;
        private readonly IStringLocalizer<AddEditProductCommandHandler> _localizer;

        public AddEditProductCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IUploadService uploadService, IStringLocalizer<AddEditProductCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _uploadService = uploadService;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AddEditProductCommand command, CancellationToken cancellationToken)
        {
            if (await _unitOfWork.Repository<Product>().Entities.Where(p => p.Id != command.Id)
                .AnyAsync(p => p.Code == command.Code, cancellationToken))
            {
                return await Result<int>.FailAsync(_localizer["Code already exists."]);
            }

            var uploadRequest = command.UploadRequest;


            if (command.Id == 0)
            {
                var product = _mapper.Map<Product>(command);
                
                if (uploadRequest != null && uploadRequest.Data != null)
                {
                    uploadRequest.FileName = $"P-{command.Code}-{product.Guid}{uploadRequest.Extension}";
                    product.ImageDataURL = _uploadService.Upload(uploadRequest);
                }
                try
                {
                    product.BaseEntityType = BaseEntityType.Product;
                    product.PatternId = (product.PatternId == 0)? null : product.PatternId;
                    product.TemplateId = (product.TemplateId == 0) ? null : product.TemplateId;

                    product.WeightMUId = (product.WeightMUId == 0) ? null : product.WeightMUId;
                    product.HeightMUId = (product.HeightMUId == 0) ? null : product.HeightMUId;
                    product.WidthMUId = (product.WidthMUId == 0) ? null : product.WidthMUId;
                    product.DiameterMUId = (product.DiameterMUId == 0) ? null : product.DiameterMUId;
                    product.LengthMUId = (product.LengthMUId == 0) ? null : product.LengthMUId;
                    product.BaseEntityType = BaseEntityType.Product;


                    await _unitOfWork.Repository<Product>().AddAsync(product);
                    await _unitOfWork.Commit(cancellationToken);

                }
                catch (System.Exception ex)
                {
                    string s = ex.Message;
                    throw;
                }
                return await Result<int>.SuccessAsync(product.Id, _localizer["Product Saved"]);
            }
            else
            {
                var product = await _unitOfWork.Repository<Product>().GetByIdAsync(command.Id);
                if (product != null)
                {
                    product.Name = command.Name ?? product.Name;
                    product.Description = command.Description ?? product.Description;
                    product.Code = command.Code ?? product.Code;
                    
                    if (uploadRequest != null && uploadRequest.Data != null)
                    {
                        uploadRequest.FileName = $"P-{command.Code}-{command.Guid}{uploadRequest.Extension}";
                        product.ImageDataURL =  _uploadService.Upload(uploadRequest);
                    }
                    else
                        product.ImageDataURL = "";

                    product.PatternId = (command.PatternId == 0) ? product.PatternId : command.PatternId;
                    product.CategoryId = (command.CategoryId == 0) ? product.CategoryId : command.CategoryId;
                    product.MeasurementUnitId = (command.MeasurementUnitId == 0) ? product.MeasurementUnitId : command.MeasurementUnitId;


                    product.WeightMUId = (command.WeightMUId == 0) ? null : command.WeightMUId;
                    product.HeightMUId = (command.HeightMUId == 0) ? null : command.HeightMUId;
                    product.DiameterMUId = (command.DiameterMUId == 0) ? null : command.DiameterMUId;
                    product.LengthMUId = (command.LengthMUId == 0) ? null : command.LengthMUId;
                    product.WidthMUId = (command.WidthMUId == 0) ? null : command.WidthMUId;


                    product.Weight = command.Weight;
                    product.Height = command.Height;
                    product.Diameter = command.Diameter;
                    product.Length= command.Length; 
                    product.Width= command.Width;

                    product.DiameterTolerance = command.DiameterTolerance;
                    product.HeightTolerance = command.HeightTolerance;
                    product.WeightTolerance = command.WeightTolerance;
                    product.LengthTolerance = command.LengthTolerance;
                    product.WidthTolerance = command.WidthTolerance;


                    product.Type = command.Type;
                    product.Kind = command.Kind;
                    product.TemplateState = command.TemplateState;
                    product.TemplateId = (command.TemplateId == 0) ? product.TemplateId : command.TemplateId;

                    //product.BoolProperty1 = (command.TemplateId == 0) ? product.TemplateId : command.TemplateId;


                    await _unitOfWork.Repository<Product>().UpdateAsync(product);
                    await _unitOfWork.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(product.Id, _localizer["Product Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Product Not Found!"]);
                }
            }
        }
    }
}