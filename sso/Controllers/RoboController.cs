using sso.Models;
using sso.Models.Data;
using sso.ViewModel;
using sso.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using System.Data.Entity;

namespace sso.Controllers
{
    public class RoboController : Controller
    {
        // GET: Robo
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AppSettings(string configFile)
        {
            var roboConfig = XmlHandler.ListarParametros(configFile);

            return View(roboConfig);
        }

        public ActionResult EditSettings(string key)
        {
            return View(XmlHandler.ConsultarNoXml(key));
        }

        [HttpPost]
        public ActionResult EditSettings(string key, string value)
        {
            XmlHandler.EditarChaveValorArquivoConfiguracao(key, value, "roboSicaqSettings");
            return View();
        }

        public ActionResult Robos()
        {
            var dados = new List<UsuarioLoginModel>();
            using (var db = new RoboContext())
            {
                dados = db.TB_LOGIN_ROBO.AsNoTracking()
                    .Where(e => e.DS_PROPOSTA_UF == "DF")
                    .AsParallel()
                    .Select(t => new UsuarioLoginModel
                    {
                        Id = t.CO_SEQ_USUARIO,
                        strUsuario = t.DS_NOME,
                        strSenha = t.DS_SENHA,
                        Convenio = t.DS_CONVENIO,
                        PropostaUF = t.DS_PROPOSTA_UF,
                        Sistema = t.DS_SISTEMA,
                        DataDesativacao = t.DT_DESATIVACAO,
                        DataExecucao = t.DT_EXECUCAO
                    }).OrderBy(s => s.Sistema).ToList();
            }

            var roboLista = new List<RoboExecucaoViewModel>();

            foreach (var item in dados)
            {
                string Sistema = item.Sistema[0] + item.Sistema.Substring(1, (item.Sistema.Length - 1)).ToLower();

                roboLista.Add(new RoboExecucaoViewModel
                {
                    Sistema = Sistema,
                    Nome = string.Format("Movix.{0}.AlteracaoSenha.Service", Sistema),
                    AppSetting = string.Format("robo{0}Settings", Sistema),
                    Execucao = item.DataExecucao,
                    Desativacao = item.DataDesativacao
                });
            }

            return View(roboLista);
        }

        public ActionResult ExecutarRobo(string sistema)
        {
            DateTime agora = DateTime.Now;

            if (!int.TryParse(XmlHandler.ConsultarNoXml("hora").Value, out int _hora))
            {
                _hora = 8;
            }
            if (!int.TryParse(XmlHandler.ConsultarNoXml("minutos").Value, out int _minutos))
            {
                _minutos = 0;
            }
            if (!int.TryParse(XmlHandler.ConsultarNoXml("segundos").Value, out int _segundos))
            {
                _segundos = 20;
            }

            DateTime horaExecucao = new DateTime(agora.Year, agora.Month, agora.Day, _hora, _minutos, _segundos);

            var result = new RoboExecucaoViewModel
            {
                Nome = string.Format("Movix.{0}.AlteracaoSenha.Service", sistema),
                Execucao = horaExecucao,
                Desativacao = horaExecucao,
                AppSetting = string.Format("robo{0}Settings", sistema),
                Sistema = sistema
            };
            var _context = new RoboContext();
            var resultList = _context.TB_LOGIN_ROBO
                .Where(x => x.DS_SISTEMA.ToLower() == sistema.ToLower())
                .Select(s => new SelectListItem
                {
                    Text = s.DS_NOME + " - " + s.DS_PROPOSTA_UF,
                    Value = s.CO_SEQ_USUARIO.ToString(),
                    Selected = false,
                });

            result.UsuarioLoginSelect = resultList;

            return View(result);
        }

        [HttpPost]
        public ActionResult ExecutarRobo(RoboExecucaoViewModel roboExecucao)
        {
            try
            {
                using (var db = new RoboContext())
                {
                    var registro = db.TB_LOGIN_ROBO.Find(roboExecucao.UsuarioId);
                    if (registro != null)
                    {
                        registro.DT_DESATIVACAO = roboExecucao.Execucao.Value.AddSeconds(-10);
                        db.Entry(registro).State = EntityState.Modified;
                        db.SaveChanges();

                        XmlHandler.EditarChaveValorArquivoConfiguracao("hora", roboExecucao.Execucao.Value.Hour.ToString(), roboExecucao.AppSetting);
                        XmlHandler.EditarChaveValorArquivoConfiguracao("minutos", roboExecucao.Execucao.Value.Minute.ToString(), roboExecucao.AppSetting);
                        XmlHandler.EditarChaveValorArquivoConfiguracao("segundos", roboExecucao.Execucao.Value.Second.ToString(), roboExecucao.AppSetting);

                        WindowsServiceHandler.RestartService(string.Format("Movix.{0}.AlteracaoSenha.Service", roboExecucao.Sistema), 5000);
                        ViewBag.Mensagem = string.Format("Mensagem: O serviço Movix.{0}.AlteracaoSenha.Service agendado para. {1}",roboExecucao.Sistema,  roboExecucao.Execucao );
                    }

                }
            }
            catch (Exception ex)
            {
                ViewBag.Mensagem = string.Format("Message:{0} \n| InnerException:{1}",ex.Message, ex.InnerException);
                //throw ex;
            }

            var _context = new RoboContext();
            roboExecucao.UsuarioLoginSelect = _context.TB_LOGIN_ROBO
                .Where(x => x.DS_SISTEMA.ToLower() == roboExecucao.Sistema.ToLower())
                .Select(s => new SelectListItem
                {
                    Text = s.DS_NOME + " - " + s.DS_PROPOSTA_UF,
                    Value = s.CO_SEQ_USUARIO.ToString(),
                    Selected = false,
                });

            return View(roboExecucao);
        }


    }
}