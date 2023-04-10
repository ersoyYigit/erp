using ArdaManager.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Application.Features.Persons.Queries.GetAllPaged
{
    public  class GetAllPagedPersonsResponse
    {
        public int Id { get; set; }
        public string Code { get; set; }
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

        public string FullName { get; set; }

        public string ImageDataURL { get; set; }


        public int? CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}
