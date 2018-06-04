using System.ComponentModel.DataAnnotations;

namespace sso.ViewModel
{
    public class UsuarioLoginSicaqViewModel
    {
        [Display(Name="Código do Convênio"), MaxLength(9), Required]
        public string convenio { get; set; }
        [Display(Name = "Identificação do Operador"), MaxLength(9), Required]
        public string login { get; set; }
        [Display(Name = "Senha do Operador"), MaxLength(9), Required]
        public string password { get; set; }
        public bool Bloqueado { get; set; }
    }
}