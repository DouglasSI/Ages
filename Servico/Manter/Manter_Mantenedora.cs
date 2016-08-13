using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Servico.Manter
{
    public class Manter_Mantenedora
    {
        public Manter_Mantenedora()
        {
            entidade = new db_agesEntities2();
        }
        private db_agesEntities2 entidade;
        public string getFantasiaById(int id)
        {
            return entidade.tb_mantenedora.Where(f => f.id.Equals(id) ).FirstOrDefault().nome_fantasia.ToUpper();
        }
        public List<tb_mantenedora> obterMantenedoras()
        {
            return entidade.tb_mantenedora.ToList();
        }
        public void cadastrar(tb_mantenedora objeto)
        {
            entidade.tb_mantenedora.Add(objeto);
            entidade.SaveChanges();
        }
        public void editar(tb_mantenedora objeto)
        {
            entidade = new db_agesEntities2();
            entidade.tb_mantenedora.Attach(objeto);
            var entry = entidade.Entry(objeto);
            entry.State = System.Data.Entity.EntityState.Modified;
            entidade.SaveChanges();
        }
        public void remover(int id)
        {

            entidade.tb_mantenedora.Remove(obterMantenedora(id));
            entidade.SaveChanges();
        }
        public tb_mantenedora obterMantenedora(int filtro)
        {
            tb_mantenedora projeto = entidade.tb_mantenedora.Where(f => f.id.Equals(filtro)).FirstOrDefault();
            return projeto;
        }

    }
}
