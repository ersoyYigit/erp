namespace ArdaManager.Client.Infrastructure.Routes
{
    public static class ChatEndpoint
    {
        public static string GetAvailableUsers = BaseEndpoint.Server + "api/chats/GetChatUsersAsync";
        public static string GetUserNotificationCountsAsync = BaseEndpoint.Server + "api/chats/GetUserNotificationsAsync";
        public static string GetUserNotificationUnreadCountsAsync = BaseEndpoint.Server + "api/chats/GetUserNotificationUnreadCountsAsync";
        public static string GetUserNotificationsAsync = BaseEndpoint.Server + "api/chats/GetUserNotificationsAsync";

        public static string SaveMessage = BaseEndpoint.Server + "api/chats/SaveMessageAsync";
        public static string MarkAsReadAsync = BaseEndpoint.Server + "api/chats/MarkAsReadAsync";

        public static string GetChatHistory(string userId)
        {
            return BaseEndpoint.Server + $"api/chats/GetChatHistoryAsync/{userId}";
        }
        
    }
}