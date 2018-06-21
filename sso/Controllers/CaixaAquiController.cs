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
        private bool _desbloquearUsuario;
        
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
            if (!bool.TryParse(ConfigurationManager.AppSettings.Get("SicaqDesbloquearUsuario"), out _desbloquearUsuario))
            {
                _desbloquearUsuario = false;
            }
            model.Bloqueado = _bloqueado;
            model.DesbloquearUsuario = _desbloquearUsuario;
            if (_bloqueado)
            {
                return View(model);
            }
            else if (_desbloquearUsuario)
            {
                return RedirectToAction("AlteracaoSenha", model);
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
            if (!bool.TryParse(ConfigurationManager.AppSettings.Get("SicaqDesbloquearUsuario"), out _desbloquearUsuario))
            {
                _desbloquearUsuario = false;
            }
            config.Bloqueado = _bloqueado;
            config.DesbloquearUsuario = _desbloquearUsuario;
            return View(config);
        }
        [HttpPost]
        public ActionResult UsuarioConfig(SicaqUsuarioConfigModel config)
        {
            try
            {
                config.Bloqueado = config.Bloqueado;
                var currentconfig = WebConfigurationManager.OpenWebConfiguration("~");
                XmlHandler.WriteSetting(currentconfig,"SicaqUsuarioBloqueado", config.Bloqueado.ToString());
                XmlHandler.WriteSetting(currentconfig, "SicaqDesbloquearUsuario", config.DesbloquearUsuario.ToString());
                //XmlHandler.SetWebAppSettings(currentconfig,"SicaqUsuarioBloqueado", config.Bloqueado.ToString());
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