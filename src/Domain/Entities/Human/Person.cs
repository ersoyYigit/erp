using ArdaManager.Domain.Contracts;
using ArdaManager.Domain.Entities.Misc;
using ArdaManager.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Domain.Entities.Human
{
    [Table("Persons")]
    public class Person : BaseEntity
    {
        
        public string MiddleName { get; set; }
        public string Surname { get; set; }
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


        [Column(TypeName = "text")]
        public string ImageDataURL { get; set; }

        [NotMapped]
        public virtual string FullName { 
            get { return Title + " " + Name + " " + MiddleName + " " + Surname; }
        }

        public int? CategoryId { get; set; }
        public virtual Category Category { get; set; }
    }
}
