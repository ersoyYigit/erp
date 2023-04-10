using ArdaManager.Domain.Entities.Misc;
using ArdaManager.Domain.Enums;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ArdaManager.Application.Features.Categories.Queries.GetAll
{
    public class GetAllCategoriesResponse
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public CategoryType Type { get; set; }
        public int? ParentCategoryId { get; set; }
        public List<Category> SubCategories { get; set; }
    }
}
