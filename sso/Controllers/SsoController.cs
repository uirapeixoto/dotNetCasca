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
    public class SsoController : Controller
    {
        public bool _usuarioBloqueado;
        public bool _recadastrarSenha;
        public bool _foiRevalidado;
        public bool _foiEditado;

        public SsoController()
        {
            if (!bool.TryParse(ConfigurationManager.AppSettings.Get("UsuarioBloqueado"), out _usuarioBloqueado))
            {
                _usuarioBloqueado = false;
            }
            if (!bool.TryParse(ConfigurationManager.AppSettings.Get("RecadastrarSenha"), out _recadastrarSenha))
            {
                _recadastrarSenha = false;
            }

            if (!bool.TryParse(ConfigurationManager.AppSettings.Get("Revalidado"), out _foiRevalidado))
            {
                _foiRevalidado = false;
            }
            if (!bool.TryParse(ConfigurationManager.AppSettings.Get("Editado"), out _foiEditado))
            {
                _foiEditado = false;
            }
        }
        // GET: Sso
        public ActionResult Index()
        {
            var usuario = new UsuarioLoginModel
            {
                RecadastrarSenha = false,
                UsuarioBloqueado = false
            };
            return View(usuario);
        }

        [HttpPost]
        public ActionResult Index(UsuarioLoginModel usuario)
        {
            if (!bool.TryParse(ConfigurationManager.AppSettings.Get("RecadastrarSenha"), out _recadastrarSenha))
            {
                _recadastrarSenha = false;
            }
            if (!bool.TryParse(ConfigurationManager.AppSettings.Get("UsuarioBloqueado"), out _usuarioBloqueado))
            {
                _usuarioBloqueado = false;
            }
            usuario.RecadastrarSenha = _recadastrarSenha;
            usuario.UsuarioBloqueado = _usuarioBloqueado;

            return View(usuario);
        }

        public ActionResult TrocaSenha()
        {
            var usuario = new UsuarioLoginPage();
            return View(usuario);
        }
        [HttpPost]
        public ActionResult TrocaSenha(UsuarioLoginPage usuario)
        {
            usuario.foiEditado = _foiEditado;
            return RedirectToAction("Index");
        }

        public ActionResult RevalidarSenha()
        {
            var usuario = new UsuarioLoginPage
            {
                foiRevalidado = _foiRevalidado
            };
            return View(usuario);
        }
        [HttpPost]
        public ActionResult RevalidarSenha(UsuarioLoginPage usuario)
        {
            usuario.foiRevalidado = _foiRevalidado;
            return View(usuario);
        }

        public ActionResult UsuarioConfig()
        {
            if (!bool.TryParse(ConfigurationManager.AppSettings.Get("UsuarioBloqueado"), out _usuarioBloqueado))
            {
                _usuarioBloqueado = false;
            }
            if (!bool.TryParse(ConfigurationManager.AppSettings.Get("RecadastrarSenha"), out _recadastrarSenha))
            {
                _recadastrarSenha = false;
            }
            if (!bool.TryParse(ConfigurationManager.AppSettings.Get("Revalidado"), out _foiRevalidado))
            {
                _foiRevalidado = false;
            }
            if (!bool.TryParse(ConfigurationManager.AppSettings.Get("Editado"), out _foiEditado))
            {
                _foiEditado = false;
            }

            var usuario = new CiwebUsuarioConfigViewModel
            {
                UsuarioBloqueado = _usuarioBloqueado,
                RecadastrarSenha = _recadastrarSenha,
                FoiEditado = _foiEditado,
                FoiValidado = _foiRevalidado,
                SituacaoUsuario = _usuarioBloqueado
            };

            return View(usuario);
        }
        [HttpPost]
        public ActionResult UsuarioConfig(CiwebUsuarioConfigViewModel config)
        {
            try
            {
                var currentconfig = WebConfigurationManager.OpenWebConfiguration("~");

                XmlHandler.WriteSetting(currentconfig, "UsuarioBloqueado", config.UsuarioBloqueado.ToString());
                XmlHandler.SetWebAppSettings(currentconfig, "UsuarioBloqueado", config.UsuarioBloqueado.ToString());
                XmlHandler.SetWebAppSettings(currentconfig, "RecadastrarSenha", config.RecadastrarSenha.ToString());
                XmlHandler.WriteSetting(currentconfig, "RecadastrarSenha", config.RecadastrarSenha.ToString());
                XmlHandler.SetWebAppSettings(currentconfig, "Revalidado", config.FoiValidado.ToString());
                XmlHandler.SetWebAppSettings(currentconfig, "Editado", config.FoiEditado.ToString());

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