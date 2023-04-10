using System;
using System.Collections.Generic;
using ArdaManager.Domain.Contracts;
using Microsoft.AspNetCore.Identity;

namespace ArdaManager.Infrastructure.Models.Identity
{
    public class VappRole : IdentityRole, IAuditableEntity<string>
    {
        public string Description { get; set; }
        public string Department { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public virtual ICollection<VappRoleClaim> RoleClaims { get; set; }

        public VappRole() : base()
        {
            RoleClaims = new HashSet<VappRoleClaim>();
        }

        public VappRole(string roleName, string roleDescription = null, string department = null) : base(roleName)
        {
            RoleClaims = new HashSet<VappRoleClaim>();
            Description = roleDescription;
            Department = department;
        }
    }
}