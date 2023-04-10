namespace ArdaManager.Client.Infrastructure.Routes
{
    public static class RolesEndpoints
    {
        public static string Delete = BaseEndpoint.Server + "api/identity/role";
        public static string GetAll = BaseEndpoint.Server + "api/identity/role";
        public static string Save = BaseEndpoint.Server + "api/identity/role";
        public static string GetPermissions = BaseEndpoint.Server + "api/identity/role/permissions/";
        public static string UpdatePermissions = BaseEndpoint.Server + "api/identity/role/permissions/update";
    }
}