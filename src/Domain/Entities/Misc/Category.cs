using ArdaManager.Domain.Contracts;
using ArdaManager.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ArdaManager.Domain.Entities.Misc
{
    public class Category : AuditableEntity<int>
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public CategoryType Type { get; set; }
        public int? ParentCategoryId { get; set; }
        [JsonIgnore]
        public Category ParentCategory { get; set; }
        public ICollection<Category> SubCategories { get; set; }


        
        public Category GetRoot()
        {
            var current = this;
            while (current.ParentCategory != null)
            {
                current = current.ParentCategory;
            }
            return current;
        }

        public int? SubCategoriesCount()
        {
            //var ret = null;
            var current = this;
            int? ret = current.SubCategories?.Count;
            return ret;
        }
    }
}
