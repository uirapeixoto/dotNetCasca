using System.ComponentModel.DataAnnotations;

namespace sso.ViewModel
{
    public class UsuarioLoginSicaqAlteracaoSenhaViewModel : UsuarioLoginSicaqViewModel
    {
        [Display(Name = "Senha de Acesso Atual"), MaxLength(9), Required]
        public string AssinaturaAtual { get; set; }
        [Display(Name = "Senha de Acesso Nova"), MaxLength(9), Required]
        public string AssinaturaNova { get; set; }
        [Display(Name = "Confirmação da Senha de Acesso Nova"), MaxLength(9), Required]
        public string ConfirmaAssinatura { get; set; }
         
    }
}