using AutoMapper;
using ArdaManager.Application.Features.DocumentTypes.Commands.AddEdit;
using ArdaManager.Application.Features.DocumentTypes.Queries.GetAll;
using ArdaManager.Application.Features.DocumentTypes.Queries.GetById;
using ArdaManager.Domain.Entities.Misc;

namespace ArdaManager.Application.Mappings
{
    public class DocumentTypeProfile : Profile
    {
        public DocumentTypeProfile()
        {
            CreateMap<AddEditDocumentTypeCommand, DocumentType>().ReverseMap();
            CreateMap<GetDocumentTypeByIdResponse, DocumentType>().ReverseMap();
            CreateMap<GetAllDocumentTypesResponse, DocumentType>().ReverseMap();
        }
    }
}