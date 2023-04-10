using ArdaManager.Application.Features.MeasurementUnits.Queries.GetAll;
using ArdaManager.Domain.Entities.Catalog;
using AutoMapper;

namespace ArdaManager.Application.Mappings
{
    public class MeasurementUnitProfile : Profile
    {
        public MeasurementUnitProfile()
        {
            CreateMap<GetAllMeasurementUnitsResponse, MeasurementUnit>().ReverseMap();
        }
    }
}
