using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Servico.Manter
{
    public class Manter_Contato
    {
        
         public Manter_Contato()
        {
            entidade = new Entities();
        }
        private Entities entidade;
        public List<tb_contato> obterContatos()
        {
            return entidade.tb_contato.ToList();
        }
        public void cadastrar(tb_contato objeto)
        {
            entidade.tb_contato.Add(objeto);
        }
        public void editar(tb_contato objeto)
        {
            entidade = new Entities();
            entidade.tb_contato.Attach(objeto);
            var entry = entidade.Entry(objeto);
            entry.State = System.Data.Entity.EntityState.Modified;
            entry.Property(e => e.id).IsModified = false;
            entidade.SaveChanges();
        }
        public tb_contato obterContato(object filtro)
        {
            return entidade.tb_contato.Where(f => f.id.Equals(filtro)).FirstOrDefault();
        }
    }
}
