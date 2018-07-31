using sso.Helper;
using sso.Models;
using sso.ViewModel;
using System;
using System.Collections.Generic;
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
            Dictionary<string, string> Menu = new Dictionary<string, string>();

            Menu.Add("MenuServicos", "Serviços ao Cliente");
            Menu.Add("AlteracaoSenha", "Alteração Senha de Acesso");
            Menu.Add("Relatorio", "Relatórios");
            Menu.Add("SolicitacoesDiversas", "Solicitações Diversas");

            ViewBag.Menu = Menu;

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

        public ActionResult MenuServicos()
        {
            Dictionary<string, string> Menu = new Dictionary<string, string>();

            Menu.Add("MenuNegocios", "Negócios");
            Menu.Add("Menu", "Voltar ao Menu Incial");

            ViewBag.Menu = Menu;

            return View();
        }

        public ActionResult MenuNegocios()
        {
            Dictionary<string, string> Menu = new Dictionary<string, string>();

            Menu.Add("PesquisaCadastral", "Pesquisa Cadastral");
            Menu.Add("CadastroClientes", "Cadastro de Clientes");
            Menu.Add("AvaliacaoRisco", "Financiamento Habitacional");
            Menu.Add("MenuAberturaMenutencao", "Abertura e Manutenção de Conta");
            Menu.Add("ChequeEspecialContaExiste", "Cheque Especial em Conta Existe");
            Menu.Add("CartaoCredito", "Cartão de Crédito");
            Menu.Add("CreditoDiretoCaixa", "CDC - Crédito Direto Caixa");
            Menu.Add("Caonsignacao", "Consignação");
            Menu.Add("CaixaSeguros", "Caixa Seguros");
            Menu.Add("ConsultarFormularioEnviados", "Consulta Formulários Enviaos");
            Menu.Add("Digitalizacao", "Digitalização");

            ViewBag.Menu = Menu;

            return View();
        }

        public ActionResult MenuAberturaMenutencao()
        {
            Dictionary<string, string> Menu = new Dictionary<string, string>();

            Menu.Add("PreAberturaContaInit", "Solicita Pré-Abertura de Conta/Cheque Especial");
            Menu.Add("MenuNegocios", "Voltar ao Menu Negócios");

            ViewBag.Menu = Menu;
            return View();
        }

        public ActionResult PreAberturaContaInit()
        {

            return View();
        }
    }


}