using ArdaManager.Application.Responses.Identity;
using ArdaManager.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using ArdaManager.Application.Interfaces.Chat;
using ArdaManager.Application.Models.Chat;

namespace ArdaManager.Application.Interfaces.Services
{
    public interface IChatService
    {
        Task<Result<IEnumerable<ChatUserResponse>>> GetChatUsersAsync(string userId);

        Task<IResult> SaveMessageAsync(ChatHistory<IChatUser> message);

        Task<Result<IEnumerable<ChatHistoryResponse>>> GetChatHistoryAsync(string userId, string contactId);

        Task<IResult> MarkAsReadAsync(int messageId);
        Task<Result<IEnumerable<ChatHistoryResponse>>> GetUserNotificationsAsync(string userId);
        Task<Result<ChatNotificationCountResponse>> GetUserNotificationCountsAsync(string userId);
        Task<Result<ChatNotificationCountResponse>> GetUserNotificationUnreadCountsAsync(string userId);


    }
}