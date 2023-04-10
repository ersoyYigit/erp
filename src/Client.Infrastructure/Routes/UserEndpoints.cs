namespace ArdaManager.Client.Infrastructure.Routes
{
    public static class UserEndpoints
    {
        public static string GetAll = BaseEndpoint.Server + "api/identity/user";

        public static string Get(string userId)
        {
            return BaseEndpoint.Server + $"api/identity/user/{userId}";
        }

        public static string GetUserRoles(string userId)
        {
            return BaseEndpoint.Server + $"api/identity/user/roles/{userId}";
        }

        public static string ExportFiltered(string searchString)
        {
            return BaseEndpoint.Server + $"{Export}?searchString={searchString}";
        }

        public static string Export = BaseEndpoint.Server + "api/identity/user/export";
        public static string Register = BaseEndpoint.Server + "api/identity/user";
        public static string ToggleUserStatus = BaseEndpoint.Server + "api/identity/user/toggle-status";
        public static string ForgotPassword = BaseEndpoint.Server + "api/identity/user/forgot-password";
        public static string ResetPassword = BaseEndpoint.Server + "api/identity/user/reset-password";
    }
}