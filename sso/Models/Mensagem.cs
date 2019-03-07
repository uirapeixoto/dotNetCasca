using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sso.Models
{
    public class Mensagem
    {
        public string CPFNaoLocalizado
        {
            get
            {
                return "Atençao! CPF {0} não foi localizado, acesse a opção Cadastro de Clientes para atualização.";
            }
        }
    }
}