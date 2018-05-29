using System;
using System.ComponentModel.DataAnnotations;

namespace sso.ViewModel
{
    public class RoboExecucaoViewModel
    {
        [Display(Name="Nome do Robô")]
        public string Nome { get; set; }
        [Display(Name = "Arquivo de Configuração")]
        public string AppSetting { get; set; }
        [Display(Name = "Data/hora da Execução")]
        public DateTime? Execucao { get; set; }
        [Display(Name = "Data/hora da Desativação")]
        public DateTime? Desativacao { get; set; }
        [Display(Name = "Sistema")]
        public string Sistema { get; internal set; }
        
    }
}