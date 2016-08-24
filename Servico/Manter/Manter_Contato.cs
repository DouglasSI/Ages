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
            entidade = new db_agesEntities2();
        }
        private db_agesEntities2 entidade;
        public List<tb_contato> obterContatos()
        {
            return entidade.tb_contato.ToList();
        }
        public tb_contato cadastrar(tb_contato objeto)
        {
            tb_contato cont  =
            entidade.tb_contato.Add(objeto);
            entidade.SaveChanges();
            return cont;
        }
        public void editar(tb_contato objeto, int id)
        {

            using (db_agesEntities2 context = new db_agesEntities2())
            {
                tb_contato contato = new tb_contato()
                {
                    id = id,
                    telefone = objeto.telefone,
                    fax = objeto.fax,
                    celular = objeto.celular,
                    email = objeto.email,
                    site = objeto.site,

                };
                context.tb_contato.Attach(contato);
                var entry = context.Entry(contato);
                entry.State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();

            }
        }
       
        public tb_contato obterContato(object filtro)
        {
            return entidade.tb_contato.Where(f => f.id.Equals(filtro)).FirstOrDefault();
        }
    }
}
