using ArdaManager.Infrastructure.Models.Identity;
using ArdaManager.Application.Specifications.Base;

namespace ArdaManager.Infrastructure.Specifications
{
    public class UserFilterSpecification : HeroSpecification<VappUser>
    {
        public UserFilterSpecification(string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = p => p.FirstName.Contains(searchString) || p.LastName.Contains(searchString) || p.Email.Contains(searchString) || p.PhoneNumber.Contains(searchString) || p.UserName.Contains(searchString);
            }
            else
            {
                Criteria = p => true;
            }
        }
    }
}