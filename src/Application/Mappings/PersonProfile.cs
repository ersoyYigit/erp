using ArdaManager.Application.Features.Persons.Commands.AddEdit;
using ArdaManager.Application.Features.Persons.Queries.GetAllPaged;
using ArdaManager.Domain.Entities.Human;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Application.Mappings
{
    public class PersonProfile : Profile
    {
        public PersonProfile()
        {
            CreateMap<AddEditPersonCommand, Person>().ReverseMap();
            //CreateMap<GetCategoryByIdResponse, Person>().ReverseMap();
            //CreateMap<GetCategoriesByTypeResponse, Person>().ReverseMap();
            CreateMap<GetAllPagedPersonsResponse, Person>().ReverseMap();
        }
    }
}
