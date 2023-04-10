using ArdaManager.Application.Interfaces.Repositories;
using ArdaManager.Domain.Entities.Addressing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Infrastructure.Repositories
{
    public class CityRepository : ICityRepository
    {
        private readonly IRepositoryAsync<City, int> _repository;

        public CityRepository(IRepositoryAsync<City, int> repository)
        {
            _repository = repository;
        }

        public async Task<List<City>> GetCitiesByCountryId(int CountryId)
        {
            List<City> ret = await Task.Run(() => _repository.Entities.Where(c => c.CountryId == CountryId).ToList());

            return  ret; 
        }


        public IQueryable<City> QGetCitiesByCountryId(int CountryId)
        {
            return _repository.Entities.Where(c => c.CountryId == CountryId);



        }

    }
}
