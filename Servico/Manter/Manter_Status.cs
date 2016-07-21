using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Servico.Manter
{
    public class Manter_Status
    {
        public Manter_Status()
        {
            entidade = new Entities();
        }
        private Entities entidade;
        public tb_status_projeto obterById(int id)
        {
            return entidade.tb_status_projeto.Where(f => f.id.Equals(id) ).FirstOrDefault();
        }
        public tb_status_orcamento obterByIdOrcamento(int id)
        {
            return entidade.tb_status_orcamento.Where(f => f.id.Equals(id)).FirstOrDefault();
        }
    }
}
