using ArdaManager.Domain.Entities.Addressing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Application.Interfaces.Repositories
{
    public interface ICityRepository
    {
        Task<List<City>> GetCitiesByCountryId(int CountryId);
        IQueryable<City> QGetCitiesByCountryId(int CountryId);

    }
}
