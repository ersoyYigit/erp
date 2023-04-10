using ArdaManager.Application.Interfaces.Repositories;
using ArdaManager.Domain.Entities.Corporation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Infrastructure.Repositories
{
    
    public class CompanyRepository : ICompanyRepository
    {
        private readonly IRepositoryAsync<Company, int> _repository;

        public CompanyRepository(IRepositoryAsync<Company, int> repository)
        {
            _repository = repository;
        }

        public async Task<bool> IsCityUsed(int cityId)
        {
            return await _repository.Entities.AnyAsync(b => b.CityId == cityId);
        }

        public async Task<bool> IsCategoryUsed(int categoryId)
        {

            return await _repository.Entities.AnyAsync(b => b.CategoryId == categoryId);
        }
    }
}
