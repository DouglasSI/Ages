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
        public static tb_orcamento orcamento { get; set; }

        public static tb_servico servico { get; set; }

        public SuperOrcamento()
        {
            orcamento = new tb_orcamento();
            servico = new tb_servico();
        }

    }
    public class SuperAnexo
    {
        
        public static List<tb_anexo> anexos { get; set; }

        
        public static tb_anexo anexo { get; set; }
        public SuperAnexo()
        {
            anexos = new List<tb_anexo>();
            anexo = new tb_anexo();
        }
        public static void clearAnexos()
        {
            anexos = new List<tb_anexo>();
        }
        public static void clearAnexo()
        {
            anexo = new tb_anexo();
        }
    }
}
