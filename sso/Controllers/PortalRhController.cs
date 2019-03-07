using sso.Helper;
using sso.Models;
using sso.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace sso.Controllers
{
    public class PortalRhController : Controller
    {

        public string _usuario;
        public string _senha;

        // GET: PortalRh
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UsuarioConfig()
        {

            _usuario = ConfigurationManager.AppSettings.Get("Usuario");
            _senha = ConfigurationManager.AppSettings.Get("Senha");

            var usuario = new UsuarioConfigPortalRhViewModel
            {
                Usuario = _usuario,
                Senha = _senha
            };

            return View(usuario);
        }

        public ActionResult Auto_Principal()
        {

            return View();
        }

        

        public ActionResult IncluirMarcacaoOnline()
        {
            return View();
        }
           
        public ActionResult UsuarioConfig(UsuarioConfigPortalRhViewModel config)
        {
            try
            {
                var currentconfig = WebConfigurationManager.OpenWebConfiguration("~");
                XmlHandler.WriteSetting(currentconfig, "Usuario", config.Usuario.ToString());
                XmlHandler.WriteSetting(currentconfig, "Senha", config.Senha.ToString());
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