using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Servico.Manter
{
    public class Manter_Campi
    {
        public Manter_Campi()
        {
            entidade = new Entities();
        }
        private Entities entidade;
        public List<tb_campi> obterCampis()
        {
            return entidade.tb_campi.ToList();
        }
        public void cadastrar(tb_campi objeto)
        {
            entidade.tb_campi.Add(objeto);
        }
        public void editar(tb_campi objeto)
        {
            entidade = new Entities();
            entidade.tb_campi.Attach(objeto);
            var entry = entidade.Entry(objeto);
            entry.State = System.Data.Entity.EntityState.Modified;
            entry.Property(e => e.id).IsModified = false;
            entidade.SaveChanges();
        }
        public tb_campi obterCampi(object filtro)
        {
            return entidade.tb_campi.Where(f => f.id.Equals(filtro)).FirstOrDefault();
        }
    }
}
