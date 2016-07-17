using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Servico.Manter
{
    public class Manter_Endereco
    {
         public Manter_Endereco()
        {
            entidade = new Entities();
        }
        private Entities entidade;
        public List<tb_endereco> obterEnderecos()
        {
            return entidade.tb_endereco.ToList();
        }
        public tb_endereco cadastrar(tb_endereco objeto)
        {
            tb_endereco cont  =
            entidade.tb_endereco.Add(objeto);
            entidade.SaveChanges();
            return cont;
        }
        public void editar(tb_endereco objeto)
        {
            entidade = new Entities();
            entidade.tb_endereco.Attach(objeto);
            var entry = entidade.Entry(objeto);
            entry.State = System.Data.Entity.EntityState.Modified;
            entry.Property(e => e.id).IsModified = false;
            entidade.SaveChanges();
        }
        public tb_endereco obterEndereco(object filtro)
        {
            return entidade.tb_endereco.Where(f => f.id.Equals(filtro)).FirstOrDefault();
        }
    }
}
