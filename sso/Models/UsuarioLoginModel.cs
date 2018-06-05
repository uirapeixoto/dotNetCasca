using System;
using System.ComponentModel.DataAnnotations;

namespace sso.Models
{
    public class UsuarioLoginModel
    {
        public int Id { get; set; }
        [Display(Name = "Usuario")]
        public string strUsuario { get; set; }
        [Display(Name = "Senha")]
        public string strSenha { get; set; }
        [Display(Name = "Convênio")]
        public string Convenio { get; set; }
        [Display(Name = "UF")]
        public string PropostaUF { get; set; }
        [Display(Name = "Sistema")]
        public string Sistema { get; set; }
        [Display(Name = "Responsável")]
        public string Responsavel { get; set; }
        [Display(Name = "Data Execução")]
        public DateTime? DataExecucao { get; set; }
        [Display(Name = "Data Desativação")]
        public DateTime? DataDesativacao { get; set; }
        public string Status { get; set; }

        public bool RecadastrarSenha { get; set; }
        public bool UsuarioBloqueado { get; set; }
    }
}