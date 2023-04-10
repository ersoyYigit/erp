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
using static ArdaManager.Shared.Constants.Permission.Permissions;

namespace ArdaManager.Application.Features.Molds.Commands.AddEdit
{
    public partial class AddEditMoldCommand : IRequest<Result<int>>
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
        public int? MoldId { get; set; }
        public bool? BoolProperty1 { get; set; }
        public string String1 { get; set; }
        public ProductType ProductType { get; set; }


        /// <summary>
        /// Mold sample 
        /// </summary>

        public int? TemplatePatternId { get; set; }
        public DateTime? ProductionDate { get; set; }
        public bool IsNew { get; set; }
        public string ModelOwner { get; set; }
        public int? UsageYear { get; set; }
        public int? CompanyId { get; set; }


        public Guid Guid { get; set; }

        public int? CategoryId { get; set; }
        public UploadRequest UploadRequest { get; set; }
    }

    internal class AddEditMoldCommandHandler : IRequestHandler<AddEditMoldCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IUploadService _uploadService;
        private readonly IStringLocalizer<AddEditMoldCommandHandler> _localizer;
        private readonly ILogger<GetAllProductsQueryHandler> _logger;

        public AddEditMoldCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IUploadService uploadService, IStringLocalizer<AddEditMoldCommandHandler> localizer, ILogger<GetAllProductsQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _uploadService = uploadService;
            _localizer = localizer;
            _logger = logger;
        }

        public async Task<Result<int>> Handle(AddEditMoldCommand command, CancellationToken cancellationToken)
        {
            if (await _unitOfWork.Repository<Mold>().Entities.Where(p => p.Id != command.Id)
                .AnyAsync(p => p.Code == command.Code, cancellationToken))
            {
                return await Result<int>.FailAsync(_localizer["Code already exists."]);
            }

            var uploadRequest = command.UploadRequest;
            //await UploadImages(cancellationToken);
            if (command.Id == 0)
            {
                var mold = _mapper.Map<Mold>(command);
                if (uploadRequest != null && uploadRequest.Data != null)
                {
                    uploadRequest.FileName = $"M-{command.Code}-{mold.Guid}{uploadRequest.Extension}";
                    mold.ImageDataURL = _uploadService.Upload(uploadRequest);
                }
                try
                {
                    mold.BaseEntityType = BaseEntityType.Mold;
                    
                    mold.PatternId = command.PatternId;
                    mold.WeightMUId = command.WeightMUId;
                    mold.HeightMUId = command.HeightMUId;
                    mold.WidthMUId = command.WidthMUId;
                    mold.DiameterMUId = command.DiameterMUId;
                    mold.LengthMUId = command.LengthMUId;
                    mold.TemplatePatternId = command.TemplatePatternId;
                    mold.UsageYear = command.UsageYear;
                    mold.CompanyId = command.CompanyId;



                    await _unitOfWork.Repository<Mold>().AddAsync(mold);
                    await _unitOfWork.Commit(cancellationToken);

                }
                catch (System.Exception ex)
                {
                    string s = ex.Message;
                    throw;
                }
                return await Result<int>.SuccessAsync(mold.Id, _localizer["Mold Saved"]);
            }
            else
            {
                var mold = await _unitOfWork.Repository<Mold>().GetByIdAsync(command.Id);
                if (mold != null)
                {
                    mold.Name = command.Name ?? mold.Name;
                    mold.Description = command.Description ?? mold.Description;
                    mold.Code = command.Code ?? mold.Code;
                    if (uploadRequest != null && uploadRequest.Data != null)
                    {
                        uploadRequest.FileName = $"T-{command.Code}-{command.Guid}{uploadRequest.Extension}";
                        mold.ImageDataURL =  _uploadService.Upload(uploadRequest);
                    }
                    //else
                    //    mold.ImageDataURL = "";

                    mold.PatternId = (command.PatternId == 0) ? mold.PatternId : command.PatternId;
                    mold.CategoryId = (command.CategoryId == 0) ? mold.CategoryId : command.CategoryId;
                    mold.MeasurementUnitId = (command.MeasurementUnitId == 0) ? mold.MeasurementUnitId : command.MeasurementUnitId;


                    mold.WeightMUId = (command.WeightMUId == 0) ? null : command.WeightMUId;
                    mold.HeightMUId = (command.HeightMUId == 0) ? null : command.HeightMUId;
                    mold.DiameterMUId = (command.DiameterMUId == 0) ? null : command.DiameterMUId;
                    mold.LengthMUId = (command.LengthMUId == 0) ? null : command.LengthMUId;
                    mold.WidthMUId = (command.WidthMUId == 0) ? null : command.WidthMUId;
                    mold.TemplatePatternId = (command.TemplatePatternId == 0) ? null : command.TemplatePatternId;
                    


                    mold.Weight = command.Weight;
                    mold.Height = command.Height;
                    mold.Diameter = command.Diameter;
                    mold.Length= command.Length; 
                    mold.Width= command.Width;

                    mold.DiameterTolerance = command.DiameterTolerance;
                    mold.HeightTolerance = command.HeightTolerance;
                    mold.WeightTolerance = command.WeightTolerance;
                    mold.LengthTolerance = command.LengthTolerance;
                    mold.WidthTolerance = command.WidthTolerance;


                    mold.Type = command.ProductType;
                    mold.ProductionDate = command.ProductionDate;
                    mold.CompanyId = (command.CompanyId == 0) ? null : command.CompanyId;


                    //mold.BoolProperty1 = (command.MoldId == 0) ? mold.MoldId : command.MoldId;


                    await _unitOfWork.Repository<Mold>().UpdateAsync(mold);
                    await _unitOfWork.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(mold.Id, _localizer["Mold Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Mold Not Found!"]);
                }
            }
        }


        private async Task UploadImages(CancellationToken cancellationToken)
        {
            List<string> codeList = new List<string>();
            var molds = await _unitOfWork.Repository<Mold>().GetAllAsync();

            foreach ( var mold in molds ) 
            {
                codeList.Add(mold.Code);
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
                    var result = _uploadService.Upload(new UploadRequest { Data = fileData, FileName = newFileName, UploadType = Application.Enums.UploadType.Mold, Extension = ext });
                    if (!string.IsNullOrWhiteSpace(result))
                    { 
                        var mold = await _unitOfWork.Repository<Mold>().Entities.Where(x=> x.Code == fileNameCheck).FirstOrDefaultAsync();
                        mold.ImageDataURL = result;
                        mold.BaseEntityType = BaseEntityType.Mold;
                        mold.Guid = guid;
                        await _unitOfWork.Repository<Mold>().UpdateAsync(mold);
                        await _unitOfWork.Commit(cancellationToken);
                        stat++;
                    }
                    _logger.Log(LogLevel.Information, "######## Status : " +stat.ToString() );
                }
            }
        }
    }
}