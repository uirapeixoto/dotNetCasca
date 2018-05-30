﻿using sso.Models;
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
        public readonly RoboContext _context;

        public RoboController(RoboContext context)
        {
            _context = context;
        }
        // GET: Robo
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AppSettings(string configFile)
        {
            List<xmlAppSettingsModel> roboConfig = new List<xmlAppSettingsModel>();
            XElement xml = XElement.Load(ConfigurationManager.AppSettings.Get(configFile));
            var element = xml.Element("appSettings");
            var nodes = element.Nodes();

            foreach (var x in xml.Element("appSettings").Descendants())
            {
                xmlAppSettingsModel p = new xmlAppSettingsModel()
                {
                    key = x.Attribute("key").Value,
                    Value = x.Attribute("value").Value,
                };
                roboConfig.Add(p);
            }
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
            var agora = DateTime.Now;
            var db = new RoboContext();

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
            
            result.UsuariosLoginSelect = db.TB_LOGIN_ROBO.AsNoTracking()
                .Where(e => e.DS_SISTEMA == sistema)
                .AsParallel()
                .Select(t => new SelectListItem
                {
                    Value = t.CO_SEQ_USUARIO.ToString(),
                    Text = string.Format("{0} - {1}", t.DS_NOME, t.DS_PROPOSTA_UF),
                    Selected = false
                });

            return View(result);
        }

        [HttpPost]
        public ActionResult ExecutarRobo(RoboExecucaoViewModel roboExecucao)
        {
            try
            {
                //atualizar o banco de dados com a nova data de desativação do usuario
                var registro = _context.TB_LOGIN_ROBO.Find(roboExecucao.UsuarioId);
                if(registro != null)
                {
                    registro.DT_DESATIVACAO = roboExecucao.Desativacao.Value.AddSeconds(-5);
                    _context.Entry(registro).State = EntityState.Modified;
                    _context.SaveChanges();

                    XmlHandler.EditarChaveValorArquivoConfiguracao("hora", roboExecucao.Desativacao.Value.Hour.ToString(), roboExecucao.AppSetting);
                    XmlHandler.EditarChaveValorArquivoConfiguracao("minutos", roboExecucao.Desativacao.Value.Minute.ToString(), roboExecucao.AppSetting);
                    XmlHandler.EditarChaveValorArquivoConfiguracao("segundos", roboExecucao.Desativacao.Value.Second.ToString(), roboExecucao.AppSetting);

                    WindowsServiceHandler.RestartService("Movix.{0}.AlteracaoSenha.Service", 1000);
                }
               
            }
            catch (Exception ex)
            {

                throw;
            }
            return RedirectToAction("Robos");
        }
        
    }
}