using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace sso.Models
{
    public class UsuarioPortalRh
    {
        [Display(Name = "I.D.")]
        public string CtrlLogin1_txtEsqSenhaIdNum { get; set; }
        [Display(Name = "Senha")]
        public string CtrlLogin1_txtSenhaAlfanumerico { get; set; }
    }
}