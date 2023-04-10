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
using System.Collections.Generic;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.IO;
using ArdaManager.Application.Enums;
using ArdaManager.Application.Features.Products.Queries.GetAllPaged;
using Microsoft.Extensions.Logging;

namespace ArdaManager.Application.Features.Templates.Commands.AddEdit
{
    public partial class AddEditTemplateCommand : IRequest<Result<int>>
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
        public TemplateKind Kind { get; set; }
        public TemplateState TemplateState { get; set; }
        public int? TemplateId { get; set; }
        public bool? BoolProperty1 { get; set; }
        public string String1 { get; set; }
        public ProductType ProductType { get; set; }


        /// <summary>
        /// Template sample 
        /// </summary>

        public int? TemplatePatternId { get; set; }
        public DateTime? ProductionDate { get; set; }
        public TemplateKind TemplateKind { get; set; }
        public TemplateState State { get; set; }
        public bool? IsAlabastr { get; set; }

        public DateTime? RevisionDate { get; set; }
        public string RevisionRequester { get; set; }
        public string RevisionRequesterDepartment { get; set; }

        public DateTime? FaultDate { get; set; }
        public DateTime? FixDate { get; set; }
        public string FaultCause { get; set; }
        public string FaultFixer { get; set; }

        public string CancelCause { get; set; }
        public string Canceller { get; set; }
        public DateTime? CancelationDate { get; set; }
        public Guid Guid { get; set; }

        public int? CategoryId { get; set; }
        public UploadRequest UploadRequest { get; set; }
    }

    internal class AddEditTemplateCommandHandler : IRequestHandler<AddEditTemplateCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IUploadService _uploadService;
        private readonly IStringLocalizer<AddEditTemplateCommandHandler> _localizer;
        private readonly ILogger<GetAllProductsQueryHandler> _logger;

        public AddEditTemplateCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IUploadService uploadService, IStringLocalizer<AddEditTemplateCommandHandler> localizer, ILogger<GetAllProductsQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _uploadService = uploadService;
            _localizer = localizer;
            _logger = logger;
        }

        public async Task<Result<int>> Handle(AddEditTemplateCommand command, CancellationToken cancellationToken)
        {
            if (await _unitOfWork.Repository<Template>().Entities.Where(p => p.Id != command.Id)
                .AnyAsync(p => p.Code == command.Code, cancellationToken))
            {
                return await Result<int>.FailAsync(_localizer["Code already exists."]);
            }

            var uploadRequest = command.UploadRequest;
            
            //await UploadImages(cancellationToken);
            if (command.Id == 0)
            {
                var template = _mapper.Map<Template>(command);
                
                if (uploadRequest != null && uploadRequest.Data != null)
                {
                    uploadRequest.FileName = $"T-{command.Code}-{template.Guid}{uploadRequest.Extension}";
                    template.ImageDataURL = _uploadService.Upload(uploadRequest);
                }
                try
                {
                    template.PatternId = (template.PatternId == 0)? null : template.PatternId;
                    template.TemplateId = (template.TemplateId == 0) ? null : template.TemplateId;

                    template.WeightMUId = (template.WeightMUId == 0) ? null : template.WeightMUId;
                    template.HeightMUId = (template.HeightMUId == 0) ? null : template.HeightMUId;
                    template.WidthMUId = (template.WidthMUId == 0) ? null : template.WidthMUId;
                    template.DiameterMUId = (template.DiameterMUId == 0) ? null : template.DiameterMUId;
                    template.LengthMUId = (template.LengthMUId == 0) ? null : template.LengthMUId;
                    template.TemplatePatternId = (template.TemplatePatternId == 0) ? null : template.TemplatePatternId;
                    template.BaseEntityType = BaseEntityType.Template;

                    await _unitOfWork.Repository<Template>().AddAsync(template);
                    await _unitOfWork.Commit(cancellationToken);

                }
                catch (System.Exception ex)
                {
                    string s = ex.Message;
                    throw;
                }
                return await Result<int>.SuccessAsync(template.Id, _localizer["Template Saved"]);
            }
            else
            {
                var template = await _unitOfWork.Repository<Template>().GetByIdAsync(command.Id);
                if (template != null)
                {
                    template.Name = command.Name ?? template.Name;
                    template.Description = command.Description ?? template.Description;
                    template.Code = command.Code ?? template.Code;
                    
                    if (uploadRequest != null && uploadRequest.Data != null)
                    {
                        uploadRequest.FileName = $"T-{command.Code}-{command.Guid}{uploadRequest.Extension}";
                        template.ImageDataURL =  _uploadService.Upload(uploadRequest);
                    }
                    //else
                    //    template.ImageDataURL = "";

                    template.PatternId = (command.PatternId == 0) ? template.PatternId : command.PatternId;
                    template.CategoryId = (command.CategoryId == 0) ? template.CategoryId : command.CategoryId;
                    template.MeasurementUnitId = (command.MeasurementUnitId == 0) ? template.MeasurementUnitId : command.MeasurementUnitId;


                    template.WeightMUId = (command.WeightMUId == 0) ? null : command.WeightMUId;
                    template.HeightMUId = (command.HeightMUId == 0) ? null : command.HeightMUId;
                    template.DiameterMUId = (command.DiameterMUId == 0) ? null : command.DiameterMUId;
                    template.LengthMUId = (command.LengthMUId == 0) ? null : command.LengthMUId;
                    template.WidthMUId = (command.WidthMUId == 0) ? null : command.WidthMUId;
                    template.TemplatePatternId = (template.TemplatePatternId == 0) ? null : template.TemplatePatternId;
                    template.TemplateId = (command.TemplateId == 0) ? template.TemplateId : command.TemplateId;


                    template.Weight = command.Weight;
                    template.Height = command.Height;
                    template.Diameter = command.Diameter;
                    template.Length= command.Length; 
                    template.Width= command.Width;

                    template.DiameterTolerance = command.DiameterTolerance;
                    template.HeightTolerance = command.HeightTolerance;
                    template.WeightTolerance = command.WeightTolerance;
                    template.LengthTolerance = command.LengthTolerance;
                    template.WidthTolerance = command.WidthTolerance;


                    template.Type = command.ProductType;
                    template.ProductionDate = command.ProductionDate;
                    template.TemplateState = command.TemplateState;
                    template.TemplateKind = command.TemplateKind;
                    template.State = command.State;
                    template.IsAlabastr = command.IsAlabastr;
                    template.RevisionDate= command.RevisionDate;
                    template.RevisionRequester= command.RevisionRequester;
                    template.RevisionRequesterDepartment = command.RevisionRequesterDepartment;
                    template.FaultDate= command.FaultDate;
                    template.FixDate= command.FixDate;
                    template.FaultCause = command.FaultCause;
                    template.FaultFixer = command.FaultFixer;
                    template.CancelCause = command.CancelCause;
                    template.Canceller = command.Canceller;
                    template.CancelationDate = command.CancelationDate;


                    //template.BoolProperty1 = (command.TemplateId == 0) ? template.TemplateId : command.TemplateId;


                    await _unitOfWork.Repository<Template>().UpdateAsync(template);
                    await _unitOfWork.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(template.Id, _localizer["Template Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Template Not Found!"]);
                }
            }
        }


        private async Task UploadImages(CancellationToken cancellationToken)
        {
            List<string> codeList = new List<string>();
            var templates = await _unitOfWork.Repository<Template>().GetAllAsync();

            foreach ( var template in templates ) 
            {
                codeList.Add(template.Code);
            }
            int stat = 0;
            string[] files = Directory.GetFiles("D:\\transfer");
            foreach (string file in files)
            {
                string fileName = Path.GetFileName(file);
                string fileNameCheck = Path.GetFileNameWithoutExtension(file);
                if (codeList.Contains(fileNameCheck))
                {
                    var guid = Guid.NewGuid();
                    var ext = Path.GetExtension(file);
                    string newFileName = $"T-{fileNameCheck}-{guid}{ext}";
                    // Dosyayı FTP'ye yükleyin
                    byte[] fileData = File.ReadAllBytes(file);
                    var result = _uploadService.Upload(new UploadRequest { Data = fileData, FileName = newFileName, UploadType = Application.Enums.UploadType.Template, Extension = ext });
                    if (!string.IsNullOrWhiteSpace(result))
                    { 
                        var template = await _unitOfWork.Repository<Template>().Entities.Where(x=> x.Code == fileNameCheck).FirstOrDefaultAsync();
                        template.ImageDataURL = result;
                        template.BaseEntityType = BaseEntityType.Template;
                        template.Guid = guid;
                        await _unitOfWork.Repository<Template>().UpdateAsync(template);
                        await _unitOfWork.Commit(cancellationToken);
                        stat++;
                    }
                    _logger.Log(LogLevel.Information, "######## Status : " +stat.ToString() );
                }
            }
        }
    }
}