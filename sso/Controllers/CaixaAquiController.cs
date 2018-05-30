using sso.ViewModel;
using System.Web.Mvc;

namespace sso.Controllers
{
    public class CaixaAquiController : Controller
    {
        // GET: CaixaAqui
        public ActionResult Index()
        {
            var usuario = new UsuarioLoginSicaqViewModel();
            return View(usuario);
        }

        [HttpPost]
        public ActionResult Index(UsuarioLoginSicaqViewModel model)
        {
            return RedirectToAction("Menu",model);
        }

        public ActionResult Menu(UsuarioLoginSicaqViewModel model)
        {
            return View(model);
        }

        public ActionResult Login()
        {

            return View();
        }

        public ActionResult AlteracaoSenha()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AlteracaoSenha(FormCollection form)
        {

            return RedirectToAction("MenuPrincipal");
        }

        public ActionResult MenuPrincipal()
        {

            return View();
        } 
    }
}