namespace ArdaManager.Client.Infrastructure.Routes
{
    class RoleClaimsEndpoints
    {
        public static string Delete = BaseEndpoint.Server + "api/identity/roleClaim";
        public static string GetAll = BaseEndpoint.Server + "api/identity/roleClaim";
        public static string Save = BaseEndpoint.Server + "api/identity/roleClaim";
    }
}