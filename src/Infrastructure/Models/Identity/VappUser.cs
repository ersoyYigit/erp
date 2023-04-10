using ArdaManager.Domain.Contracts;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using ArdaManager.Application.Interfaces.Chat;
using ArdaManager.Application.Models.Chat;

namespace ArdaManager.Infrastructure.Models.Identity
{
    public class VappUser : IdentityUser<string>, IChatUser, IAuditableEntity<string>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Department { get; set; }
        public string CreatedBy { get; set; }

        [Column(TypeName = "text")]
        public string ProfilePictureDataUrl { get; set; }

        public DateTime CreatedOn { get; set; }

        public string LastModifiedBy { get; set; }

        public DateTime? LastModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
        public bool IsActive { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        public virtual ICollection<ChatHistory<VappUser>> ChatHistoryFromUsers { get; set; }
        public virtual ICollection<ChatHistory<VappUser>> ChatHistoryToUsers { get; set; }

        public VappUser()
        {
            ChatHistoryFromUsers = new HashSet<ChatHistory<VappUser>>();
            ChatHistoryToUsers = new HashSet<ChatHistory<VappUser>>();
        }
    }
}