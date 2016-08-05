using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Servico.Manter
{
    public class Manter_Orcamento_Servico
    { 
        public Manter_Orcamento_Servico()
        {
            entidade = new Entities();
        }
        private Entities entidade;
        public void cadastrar(tb_orcamento_servico objeto)
        {
            
            entidade.tb_orcamento_servico.Add(objeto);

            entidade.SaveChanges();
        }
        public List<tb_orcamento_servico> ObterServicosporOrcamento(int id)
        {
            List<tb_orcamento_servico> servicos = entidade.tb_orcamento_servico.ToList().Where(f => f.id_orcamento.Equals(id)).ToList();
            return servicos;
        }
        public tb_orcamento_servico ObterServicoPorOrcamento(int id)
        {
            tb_orcamento_servico tb_orcamento_servico = entidade.tb_orcamento_servico.Where(f => f.id_orcamento.Equals(id)).FirstOrDefault();
            return tb_orcamento_servico;
        }
        public void remover(int id)
        {

            entidade.tb_orcamento_servico.Remove(ObterServicoPorOrcamento(id));
            entidade.SaveChanges();
        }
    }
}
