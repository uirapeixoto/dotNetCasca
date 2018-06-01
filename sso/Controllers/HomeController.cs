using System.Web.Mvc;

namespace sso.Controllers
{
    public class HomeController : Controller
    {
        
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CiwebParams()
        {

            return View();
        }
        
    }
}