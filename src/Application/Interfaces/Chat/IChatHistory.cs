using ArdaManager.Domain.Enums;
using ArdaManager.Domain.Enums.Doc;
using System;

namespace ArdaManager.Application.Interfaces.Chat
{
    public interface IChatHistory<TUser> where TUser : IChatUser
    {
        public long Id { get; set; }
        public string FromUserId { get; set; }
        public string ToUserId { get; set; }
        public string Message { get; set; }
        public DateTime CreatedDate { get; set; }

        public bool IsRead { get; set; }
        public int? RelatedDocId { get; set; }
        public string RelatedLink { get; set; }
        public DocType? DocType { get; set; }
        public MessageType MessageType { get; set; }

        public TUser FromUser { get; set; }
        public TUser ToUser { get; set; }
    }
}