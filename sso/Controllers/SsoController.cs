using sso.Models;
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
    }
}