using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Model.Super
{
    public class SuperOrcamento
    {
        public tb_orcamento orcamento { get; set; }

        public tb_servico servico { get; set; }

        public SuperOrcamento()
        {
            orcamento = new tb_orcamento();
            servico = new tb_servico();
        }

    }
}
