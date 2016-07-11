using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Servico.Manter
{
    public class Manter_Compra
    {
         public Manter_Compra()
        {
            entidade = new Entities();
        }
        private Entities entidade;
        public List<tb_compra> obterCompras()
        {
            return entidade.tb_compra.ToList();
        }
        public void cadastrar(tb_compra objeto)
        {
            entidade.tb_compra.Add(objeto);
        }
        public void editar(tb_compra objeto)
        {
            entidade = new Entities();
            entidade.tb_compra.Attach(objeto);
            var entry = entidade.Entry(objeto);
            entry.State = System.Data.Entity.EntityState.Modified;
            entry.Property(e => e.id).IsModified = false;
            entidade.SaveChanges();
        }
        public tb_compra obterCompra(object filtro)
        {
            return entidade.tb_compra.Where(f => f.id.Equals(filtro)).FirstOrDefault();
        }
    }
}
