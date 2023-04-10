namespace ArdaManager.Application.Requests.Identity
{
    public class ToggleUserStatusRequest
    {
        public bool ActivateUser { get; set; }
        public string Department { get; set; }
        public string UserId { get; set; }
    }
}