using System.ComponentModel.DataAnnotations;

namespace sso.ViewModel
{
    public class UsuarioConfigPortalRhViewModel
    {
        [Display(Name="I.D."), MaxLength(9), Required]
        public string Usuario { get; set; }
        [Display(Name = "Senha"), MaxLength(9), Required]
        public string Senha { get; set; }
    }
}