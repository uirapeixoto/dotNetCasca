using sso.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sso.Models
{
    public class DadosPropostaModel
    {
        public TipoContaBancariaEnum tipoConta { get; set; }
        public TipoCartaoEnum tipoCartao { get; set; }
        public int contaCCheque { get; set; }
        public string chequeEspecial { get; set; }
        public string prazo { get; set; }
        public string nrConta { get; set; }
        public string taxaMensalJuros { get; set; }
        public string taxaAnualJuros { get; set; }
        public string CetMensal { get; set; }
        public string CetAnual{ get; set; }

    }
}