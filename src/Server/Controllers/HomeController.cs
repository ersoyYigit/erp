using Microsoft.AspNetCore.Mvc;

namespace ArdaManager.Server.Controllers
{
    public class HomeController : Controller
    {
        public RedirectResult Index()
        {
            return Redirect("/swagger/index.html");
        }
    }
}
