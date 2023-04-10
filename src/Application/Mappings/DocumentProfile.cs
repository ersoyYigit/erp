using AutoMapper;
using ArdaManager.Application.Features.Documents.Commands.AddEdit;
using ArdaManager.Application.Features.Documents.Queries.GetById;
using ArdaManager.Domain.Entities.Misc;

namespace ArdaManager.Application.Mappings
{
    public class DocumentProfile : Profile
    {
        public DocumentProfile()
        {
            CreateMap<AddEditDocumentCommand, Document>().ReverseMap();
            CreateMap<GetDocumentByIdResponse, Document>().ReverseMap();
        }
    }
}