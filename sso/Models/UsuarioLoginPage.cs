using System.ComponentModel.DataAnnotations;

namespace sso.Models
{
    public class UsuarioLoginPage
    {
        [Display(Name = "Usuário")]
        public string strUsuario
        {
            get
            {
                return "X557494";
            }
        }
        [Display(Name = "Senha")]
        public string strAtual { get; set; }
        [Display(Name = "Nova Senha")]
        public string strNova { get; set; }
        [Display(Name = "Confirme a senha")]
        public string strConfirma { get; set; }
        [Display(Name = "Primeiro nome")]
        public string strNome { get; set; }

        public bool foiEditado { get; set; }
        public bool foiRevalidado { get; set; }

        public string strPut { get
            {
                return "EEE3ED4EBEC052BCDF8854EEED54BD88";
            }
        }
        public string strPush {
            get
            {
                return "8EA27200EE08FE805A128E8B8BAC1769";
            }
        }
        public string j_idt10 {
            get
            {
                return "j_idt10";
            }
        }
    }
}