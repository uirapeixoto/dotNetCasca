using System.ComponentModel.DataAnnotations;

namespace sso.ViewModel
{
    public class CiwebUsuarioConfigViewModel
    {
        [Display(Name="Usuário Bloqueado")]
        public bool UsuarioBloqueado { get; set; }
        [Display(Name = "Usuario")]
        public bool Usuario { get; set; }
        [Display(Name = "Senha Expirou")]
        public bool RecadastrarSenha { get; set; }
        [Display(Name = "Foi Validado")]
        public bool FoiValidado { get; set; }
        [Display(Name = "Foi Editado")]
        public bool FoiEditado { get; set; }
        [Display(Name = "Situação do Usuário")]
        public bool SituacaoUsuario { get; set; }
    }
}