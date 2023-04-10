namespace ArdaManager.Client.Infrastructure.Routes
{
    public static class TokenEndpoints
    {
        public static string Get = BaseEndpoint.Server + "api/identity/token";
        public static string Refresh = BaseEndpoint.Server + "api/identity/token/refresh";
    }
}