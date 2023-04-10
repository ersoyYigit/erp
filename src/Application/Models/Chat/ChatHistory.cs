using System;
using ArdaManager.Application.Interfaces.Chat;
using ArdaManager.Domain.Enums;
using ArdaManager.Domain.Enums.Doc;

namespace ArdaManager.Application.Models.Chat
{
    public partial class ChatHistory<TUser> : IChatHistory<TUser> where TUser : IChatUser
    {
        public long Id { get; set; }
        public string FromUserId { get; set; }
        public string FromUserName { get; set; }
        public string ToUserId { get; set; }
        public string ToUserName { get; set; }
        public string Message { get; set; }
        
        public bool IsRead { get; set; } = false;
        public int? RelatedDocId { get; set; }
        public string RelatedLink { get; set; }
        public DocType? DocType { get; set; }
        public MessageType MessageType { get; set; } = MessageType.ChatMessage;
        
        public DateTime CreatedDate { get; set; }
        public virtual TUser FromUser { get; set; }
        public virtual TUser ToUser { get; set; }
    }
}