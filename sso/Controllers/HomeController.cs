using sso.Models;
using sso.Models.Data;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web.Mvc;

namespace sso.Controllers
{
    public class HomeController : Controller
    {
        public bool _usuarioBloqueado;
        public bool _recadastrarSenha;
        public bool _foiRevalidado;
        public bool _foiEditado;

        public HomeController()
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

        public ActionResult EtrocaSenha()
        {
            return View();
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
            return RedirectToAction("Menu");
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

        public ActionResult Menu()
        {
            var dados = new List<UsuarioLoginModel>();
            using (var db = new RoboContext())
            {
                dados = db.TB_LOGIN_ROBO.AsNoTracking().AsParallel().Select(t => new UsuarioLoginModel
                {
                    Id = t.CO_SEQ_USUARIO,
                    strUsuario = t.DS_NOME,
                    strSenha = t.DS_SENHA,
                    Convenio = t.DS_CONVENIO,
                    PropostaUF = t.DS_PROPOSTA_UF,
                    Sistema = t.DS_SISTEMA,
                    Responsavel = t.DS_RESPONSAVEL,
                    DataDesativacao = t.DT_DESATIVACAO,
                    DataExecucao = t.DT_EXECUCAO
                }).OrderBy(s => s.Sistema).ToList();
            }
            return View(dados);
        }

        public ActionResult Edit(int id)
        {
            var usuario = new UsuarioLoginModel();
            using (var db = new RoboContext())
            {
                usuario = db.TB_LOGIN_ROBO
                    .AsNoTracking()
                    .Where(t => t.CO_SEQ_USUARIO == id)
                    .AsParallel()
                    .Select(t => new UsuarioLoginModel
                    {
                        Id = t.CO_SEQ_USUARIO,
                        strUsuario = t.DS_NOME,
                        strSenha = t.DS_SENHA,
                        Convenio = t.DS_CONVENIO,
                        PropostaUF = t.DS_PROPOSTA_UF,
                        Sistema = t.DS_SISTEMA,
                        Responsavel = t.DS_RESPONSAVEL,
                        DataDesativacao = t.DT_DESATIVACAO,
                        DataExecucao = t.DT_EXECUCAO
                    }).FirstOrDefault();

            }

            return View(usuario);
        }

        [HttpPost]
        public ActionResult Edit(UsuarioLoginModel usuario)
        {
            try
            {
                using (var db = new RoboContext())
                {
                    var registro = db.TB_LOGIN_ROBO.Where(t => t.CO_SEQ_USUARIO == usuario.Id).SingleOrDefault();
                    registro.DS_SENHA = usuario.strSenha;
                    registro.DT_DESATIVACAO = usuario.DataDesativacao;
                    db.Entry(registro).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            catch (DbEntityValidationException e)
            {
                IList<string> errorList = new List<string>();
                foreach (var eve in e.EntityValidationErrors)
                {
                    errorList.Add(string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                    {
                        errorList.Add(string.Format ("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                    }
                }

                throw e;
            }

            return RedirectToAction("Menu");
        }

        [HttpPost]
        public ActionResult Menu(UsuarioLoginModel usuario)
        {

            return View(new UsuarioLoginModel());
        }


    }
}