using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

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
        public string Sistema { get; set; }
        public IEnumerable<SelectListItem> UsuarioLoginSelect { get; set; }
        public int UsuarioId { get; set; }

        public RoboExecucaoViewModel()
        {
            this.UsuarioLoginSelect= new List<SelectListItem> { new SelectListItem { Selected = true, Text = "Selecione", Value = "" } };
        }
    }
}