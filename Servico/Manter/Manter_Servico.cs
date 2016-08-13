using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Servico.Manter
{
    public class Manter_Servico
    {
         public Manter_Servico()
        {
            entidade = new db_agesEntities2();
        }
        private db_agesEntities2 entidade;
        public List<tb_servico> obterServicos()
        {
            return entidade.tb_servico.ToList();
        }
        public void cadastrar(tb_servico objeto)
        {
            entidade.tb_servico.Add(objeto);
            entidade.SaveChanges();
        }
        public void editar(tb_servico objeto)
        {
            entidade = new db_agesEntities2();
            entidade.tb_servico.Attach(objeto);
            var entry = entidade.Entry(objeto);
            entry.State = System.Data.Entity.EntityState.Modified;

            entidade.SaveChanges();
        }
        public void remover(int id)
        {

            entidade.tb_servico.Remove(obterServico(id));
            entidade.SaveChanges();
        }
        public tb_servico obterServico(int filtro)
        {
            tb_servico projeto = entidade.tb_servico.Where(f => f.id.Equals(filtro)).FirstOrDefault();
            return projeto;
        }
    }
}
