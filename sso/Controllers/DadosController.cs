using sso.Models;
using sso.Models.Data;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web.Mvc;

namespace sso.Controllers
{
    public class DadosController : Controller
    {
        // GET: Dados
        public ActionResult Index()
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
                    DataExecucao = t.DT_EXECUCAO,
                    Status = t.DS_STATUS
                }).OrderBy(s => s.Sistema).ToList();
            }
            return View(dados);
        }

        [HttpPost]
        public ActionResult Index(UsuarioLoginModel usuario)
        {

            return View(new UsuarioLoginModel());
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
                ViewBag.Mensagem = "Registro alterado com sucesso.";
            }
            catch (DbEntityValidationException e)
            {
                IList<string> errorList = new List<string>();
                foreach (var eve in e.EntityValidationErrors)
                {
                    errorList.Add(string.Format("Tipo de entidade \"{0}\" no estado \"{1}\" tem o seguinte erro de validação:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                    {
                        errorList.Add(string.Format("- Propriedade: \"{0}\", Erro: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                    }
                }

                ViewBag.MensagemErro = e.Message;
            }

            return RedirectToAction("Index");
        }
    }
}