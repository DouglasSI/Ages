using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Servico.Manter
{
    public class Manter_Funcao
    {
        public Manter_Funcao()
        {
            entidade = new Entities();
        }
        private Entities entidade;
        public List<tb_funcao> obterFuncoes()
        {
            return entidade.tb_funcao.ToList();
        }
        public void cadastrar(tb_funcao objeto)
        {
            entidade.tb_funcao.Add(objeto);
            entidade.SaveChanges();
        }
        public void editar(tb_funcao objeto)
        {
            entidade.tb_funcao.Attach(objeto);
            var entry = entidade.Entry(objeto);
            entry.State = System.Data.Entity.EntityState.Modified;

            entidade.SaveChanges();
        }
        public void remover(int id)
        {

            entidade.tb_funcao.Remove(obterFuncao(id));
            entidade.SaveChanges();
        }
        public tb_funcao obterFuncao(int filtro)
        {
            tb_funcao projeto = entidade.tb_funcao.Where(f => f.id.Equals(filtro)).FirstOrDefault();
            return projeto;
        }
    }
}
