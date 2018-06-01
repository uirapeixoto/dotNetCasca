using sso.Helper;
using sso.Models;
using sso.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
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
            if (!bool.TryParse(ConfigurationManager.AppSettings.Get("Revalidado"), out _foiEditado))
            {
                _foiEditado = false;
            }
        }
        // GET: Sso
        public ActionResult Index()
        {
            var usuario = new UsuarioLoginModel
            {
                RecadastrarSenha = _usuarioBloqueado,
                UsuarioBloqueado = _recadastrarSenha
            };
            return View(usuario);
        }

        [HttpPost]
        public ActionResult Index(UsuarioLoginModel usuario)
        {
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
                config.UsuarioBloqueado = config.SituacaoUsuario;
                config.RecadastrarSenha = !config.UsuarioBloqueado;

                XmlHandler.EditarConfiguracao("UsuarioBloqueado", config.UsuarioBloqueado.ToString());
                XmlHandler.EditarConfiguracao("RecadastrarSenha", config.RecadastrarSenha.ToString());
                XmlHandler.EditarConfiguracao("Revalidado", config.UsuarioBloqueado.ToString());
                XmlHandler.EditarConfiguracao("UsuarioBloqueado", config.UsuarioBloqueado.ToString());

            }
            catch (Exception ex)
            {
                throw ex;
            }
            

            return View(config);
        }
    }
}