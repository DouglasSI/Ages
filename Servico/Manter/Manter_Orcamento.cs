using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Servico.Manter
{
    public class Manter_Orcamento
    {

        public Manter_Orcamento()
        {
            entidade = new Entities();
        }
        private Entities entidade;

        public List<tb_orcamento> obterOrcamentos(int id_projeto)
        {
            List<tb_orcamento> retur =entidade.tb_orcamento.ToList().Where(f => f.id_projeto.Equals(id_projeto)).ToList();
            return retur;
        }
        public List<tb_orcamento> obterOrcamentosAprovados(int id_projeto)
        {
            List<tb_orcamento> retur = entidade.tb_orcamento.ToList().Where(f => f.id_projeto.Equals(id_projeto) && f.id_status.Equals(2)).ToList();
            return retur;
        }

        public void cadastrar(tb_orcamento objeto)
        {
            entidade.tb_orcamento.Add(objeto);
            //entidade.SaveChangesAsync();
            entidade.SaveChanges();
        }
        public void editarStatus(tb_orcamento orcamento,  int id_status)
        {
            
            using (Entities context = new Entities())
            {
                tb_orcamento orc = new tb_orcamento()
                {
                    id = orcamento.id,
                    id_status = id_status,
                    valor = orcamento.valor,
                    anotacao = orcamento.anotacao,
                    id_empresa = orcamento.id_empresa,
                    id_projeto = orcamento.id_projeto,
                    id_usuario = orcamento.id_usuario
                };
                context.tb_orcamento.Attach(orc);
                var entry = context.Entry(orc);
                entry.State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();

            }

        }
        public void editar(tb_orcamento objeto)
        {
            
            entidade.tb_orcamento.Attach(objeto);
            var entry = entidade.Entry(objeto);
            entry.State = System.Data.Entity.EntityState.Modified;
            entidade.SaveChanges();
        }
        public void remover(int id)
        {

            entidade.tb_orcamento.Remove(obterOrcamento(id));
            entidade.SaveChanges();
        }
        public tb_orcamento obterOrcamento(int filtro)
        {
            tb_orcamento projeto = entidade.tb_orcamento.Where(f => f.id.Equals(filtro)).FirstOrDefault();
            return projeto;
        }

    }
}

