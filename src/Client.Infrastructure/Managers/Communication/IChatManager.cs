using ArdaManager.Application.Models.Chat;
using ArdaManager.Application.Responses.Identity;
using ArdaManager.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using ArdaManager.Application.Interfaces.Chat;

namespace ArdaManager.Client.Infrastructure.Managers.Communication
{
    public interface IChatManager : IManager
    {
        Task<IResult<IEnumerable<ChatUserResponse>>> GetChatUsersAsync();

        Task<IResult> SaveMessageAsync(ChatHistory<IChatUser> chatHistory);

        Task<IResult<IEnumerable<ChatHistoryResponse>>> GetChatHistoryAsync(string cId);
        Task<IResult<IEnumerable<ChatHistoryResponse>>> GetUserNotificationsAsync();
        Task<IResult<ChatNotificationCountResponse>> GetUserNotificationCountsAsync();
        Task<IResult<ChatNotificationCountResponse>> GetUserNotificationUnreadCountsAsync();
    }
}