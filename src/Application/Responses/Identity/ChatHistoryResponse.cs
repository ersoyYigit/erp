using ArdaManager.Domain.Enums.Doc;
using ArdaManager.Domain.Enums;
using System;

namespace ArdaManager.Application.Responses.Identity
{
    public partial class ChatHistoryResponse
    {
        public long Id { get; set; }
        public string FromUserId { get; set; }
        public string FromUserImageURL { get; set; }
        public string FromUserFullName { get; set; }
        public string ToUserId { get; set; }
        public string ToUserImageURL { get; set; }
        public string ToUserFullName { get; set; }
        public string Message { get; set; }

        public bool IsRead { get; set; } = false;
        public int? RelatedDocId { get; set; }
        public string RelatedLink { get; set; }
        public DocType? DocType { get; set; }
        public MessageType MessageType { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}