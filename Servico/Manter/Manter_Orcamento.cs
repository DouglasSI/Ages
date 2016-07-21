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

        public List<tb_orcamento> obterOrcamentos()
        {
            return entidade.tb_orcamento.ToList();
        }
        public void cadastrar(tb_orcamento objeto)
        {
            entidade.tb_orcamento.Add(objeto);
            entidade.SaveChanges();
        }
        public void editar(tb_orcamento objeto)
        {
            entidade = new Entities();
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

