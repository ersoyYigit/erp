using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Application.Interfaces.Repositories
{
    
    public interface ICompanyRepository
    {
        Task<bool> IsCityUsed(int cityId);
        Task<bool> IsCategoryUsed(int categoryId);

    }
}
