using sso.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sso.Models
{
    public class DadosClienteManutencaoContaModel
    {
        public string numeroCpf { get; set; }
        public string nomeCompleto { get; set; }
        public TipoContaEnum tipoContaIndividualConjunta { get; set; }
        public string numeroCpf2 { get; set; }
        public string nomeSegundoTitular { get; set; }
        public string endereco { get; set; }
        public string uf { get; set; }
        public string municipio { get; set; }

    }
}