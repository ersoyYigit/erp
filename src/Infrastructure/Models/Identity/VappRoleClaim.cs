using System;
using ArdaManager.Domain.Contracts;
using Microsoft.AspNetCore.Identity;

namespace ArdaManager.Infrastructure.Models.Identity
{
    public class VappRoleClaim : IdentityRoleClaim<string>, IAuditableEntity<int>
    {
        public string Description { get; set; }
        public string Group { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public virtual VappRole Role { get; set; }

        public VappRoleClaim() : base()
        {
        }

        public VappRoleClaim(string roleClaimDescription = null, string roleClaimGroup = null) : base()
        {
            Description = roleClaimDescription;
            Group = roleClaimGroup;
        }
    }
}