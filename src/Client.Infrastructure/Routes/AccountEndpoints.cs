namespace ArdaManager.Client.Infrastructure.Routes
{
    public static class AccountEndpoints
    {
        public static string Register = BaseEndpoint.Server + "api/identity/account/register";
        public static string ChangePassword = BaseEndpoint.Server + "api/identity/account/changepassword";
        public static string UpdateProfile = BaseEndpoint.Server + "api/identity/account/updateprofile";

        public static string GetProfilePicture(string userId)
        {
            return BaseEndpoint.Server + $"api/identity/account/profile-picture/{userId}";
        }

        public static string UpdateProfilePicture(string userId)
        {
            return BaseEndpoint.Server + $"api/identity/account/profile-picture/{userId}";
        }
    }
}