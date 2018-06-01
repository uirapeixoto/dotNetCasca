using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sso.Models
{
    public class CiwebUsuarioConfigModel
    {
        public bool UsuarioBloqueado{ get; set; }
        public bool Usuario{ get; set; }
        public bool RecadastrarSenha { get; set; }
        public bool FoiValidado { get; set; }
        public bool FoiEditado { get; set; }
    }
}