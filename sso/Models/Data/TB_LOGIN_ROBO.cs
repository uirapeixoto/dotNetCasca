namespace sso.Models.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TB_LOGIN_ROBO
    {
        [Key]
        public int CO_SEQ_USUARIO { get; set; }

        [Required]
        [StringLength(60)]
        public string DS_NOME { get; set; }

        [Required]
        [StringLength(50)]
        public string DS_SENHA { get; set; }

        [StringLength(50)]
        public string DS_CONVENIO { get; set; }

        public DateTime? DT_EXECUCAO { get; set; }

        public DateTime? DT_DESATIVACAO { get; set; }

        [Required]
        [StringLength(2)]
        public string DS_PROPOSTA_UF { get; set; }

        [Required]
        [StringLength(50)]
        public string DS_SISTEMA { get; set; }

        [StringLength(50)]
        public string DS_STATUS { get; set; }

        [StringLength(50)]
        public string DS_RESPONSAVEL { get; set; }

    }
}
