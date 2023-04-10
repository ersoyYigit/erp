using ArdaManager.Application.Models.Chat;
using ArdaManager.Application.Responses.Identity;
using ArdaManager.Client.Infrastructure.Extensions;
using ArdaManager.Shared.Wrapper;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ArdaManager.Application.Interfaces.Chat;
using System.Security.Cryptography;
using System;

namespace ArdaManager.Client.Infrastructure.Managers.Communication
{
    public class ChatManager : IChatManager
    {
        private readonly HttpClient _httpClient;

        public ChatManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<IEnumerable<ChatHistoryResponse>>> GetChatHistoryAsync(string cId)
        {
            var response = await _httpClient.GetAsync(Routes.ChatEndpoint.GetChatHistory(cId));
            var data = await response.ToResult<IEnumerable<ChatHistoryResponse>>();
            return data;
        }

        public async Task<IResult<IEnumerable<ChatUserResponse>>> GetChatUsersAsync()
        {
            var response = await _httpClient.GetAsync(Routes.ChatEndpoint.GetAvailableUsers);
            var data = await response.ToResult<IEnumerable<ChatUserResponse>>();
            return data;
        }

        public async Task<IResult<ChatNotificationCountResponse>> GetUserNotificationCountsAsync()
        {
            var response = await _httpClient.GetAsync(Routes.ChatEndpoint.GetUserNotificationCountsAsync);
            var data = await response.ToResult<ChatNotificationCountResponse>();
            return data;
        }

        public async Task<IResult<IEnumerable<ChatHistoryResponse>>> GetUserNotificationsAsync()
        {
            var response = await _httpClient.GetAsync(Routes.ChatEndpoint.GetUserNotificationsAsync);
            var data = await response.ToResult<IEnumerable<ChatHistoryResponse>>();
            return data;
        }

        public async Task<IResult<ChatNotificationCountResponse>> GetUserNotificationUnreadCountsAsync()
        {
            var response = await _httpClient.GetAsync(Routes.ChatEndpoint.GetUserNotificationCountsAsync);
            var data = await response.ToResult<ChatNotificationCountResponse>();
            return data;
        }

        public async Task<IResult> SaveMessageAsync(ChatHistory<IChatUser> chatHistory)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.ChatEndpoint.SaveMessage, chatHistory);
            var data = await response.ToResult();
            return data;
        }
    }
}