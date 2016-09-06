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
            entidade = new db_agesEntities2();
        }
        private db_agesEntities2 entidade;
        public void cadastrar(tb_orcamento_servico objeto)
        {
            
            entidade.tb_orcamento_servico.Add(objeto);

            entidade.SaveChanges();
        }
        public void Editar(tb_orcamento_servico orc)
         {
             using (db_agesEntities2 context = new db_agesEntities2())
             {
                 tb_orcamento_servico orc_serv = new tb_orcamento_servico()
                 {
                     id_orcamento = orc.id_orcamento,
                     id_servico = orc.id_servico,
                     data_cadastro = orc.data_cadastro,
                     anotacao = orc.anotacao,
                     valor_servico = orc.valor_servico,
                 };
                 context.tb_orcamento_servico.Attach(orc_serv);
                 var entry = context.Entry(orc_serv);
                 entry.State = System.Data.Entity.EntityState.Modified;
                 context.SaveChanges();
             
             }


        }
        public List<tb_orcamento_servico> ObterServicosporOrcamento(int id)
        {
            List<tb_orcamento_servico> servicos = entidade.tb_orcamento_servico.ToList().Where(f => f.id_orcamento.Equals(id)).ToList();
            return servicos;
        }
        public tb_orcamento_servico ObterServicoPorOrcamento(int? id)
        {
            tb_orcamento_servico tb_orcamento_servico = entidade.tb_orcamento_servico.Where(f => f.id_orcamento.Equals((int)id)).FirstOrDefault();
            return tb_orcamento_servico;
        }
        public tb_orcamento_servico ObterOrcamentoServico (int? id_servico, int? id_orcamento)
        {
            tb_orcamento_servico tb_orcamento_servico = entidade.tb_orcamento_servico.Where(f => f.id_orcamento.Equals((int)id_orcamento) && f.id_servico.Equals((int)id_servico)).FirstOrDefault();
            return tb_orcamento_servico;
        }
        public void remover(int? id)
        {

            entidade.tb_orcamento_servico.Remove(ObterServicoPorOrcamento((int)id));
            entidade.SaveChanges();
        }
    }
}
