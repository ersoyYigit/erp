using ArdaManager.Application.Interfaces.Repositories;
using ArdaManager.Domain.Entities.Catalog;

namespace ArdaManager.Infrastructure.Repositories
{
    public class PatternRepository : IPatternRepository
    {
        private readonly IRepositoryAsync<Pattern, int> _repository;

        public PatternRepository(IRepositoryAsync<Pattern, int> repository)
        {
            _repository = repository;
        }
    }
}