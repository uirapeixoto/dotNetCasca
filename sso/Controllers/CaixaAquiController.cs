using sso.Helper;
using sso.Models;
using sso.ViewModel;
using System;
using System.Configuration;
using System.Web.Configuration;
using System.Web.Mvc;

namespace sso.Controllers
{
    public class CaixaAquiController : Controller
    {
        private bool _bloqueado;
        
        // GET: CaixaAqui
        public ActionResult Index()
        {
            var usuario = new UsuarioLoginSicaqViewModel();
            return View(usuario);
        }

        [HttpPost]
        public ActionResult Index(UsuarioLoginSicaqViewModel model)
        {
            if(!bool.TryParse(ConfigurationManager.AppSettings.Get("SicaqUsuarioBloqueado"), out _bloqueado))
            {
                _bloqueado = false;
            }
            model.Bloqueado = _bloqueado;
            if (_bloqueado)
            {
                return View(model);
            }
            else
            {
                return RedirectToAction("Menu", model);
            }
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
        public ActionResult UsuarioConfig()
        {
            var config = new SicaqUsuarioConfigModel();
            ConfigurationManager.RefreshSection("appSettings");
            if (!bool.TryParse(ConfigurationManager.AppSettings.Get("SicaqUsuarioBloqueado"), out _bloqueado))
            {
                _bloqueado = false;
            }
            config.Bloqueado = _bloqueado;
            return View(config);
        }
        [HttpPost]
        public ActionResult UsuarioConfig(SicaqUsuarioConfigModel config)
        {
            try
            {
                config.Bloqueado = config.Bloqueado;
                var currentconfig = WebConfigurationManager.OpenWebConfiguration("~");

                XmlHandler.SetAppSettings(currentconfig, "SicaqUsuarioBloqueado", config.Bloqueado.ToString());
                ViewBag.Mensagem = "Registro alterado com sucesso.";

            }
            catch (Exception ex)
            {
                ViewBag.MensagemErro = ex.Message;
                //throw ex;
            }


            return View(config);
        }
    }
}