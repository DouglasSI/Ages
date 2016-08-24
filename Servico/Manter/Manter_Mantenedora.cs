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
        public void editar(tb_mantenedora objeto, int id_end, int id_cont)
        {

            using (db_agesEntities2 context = new db_agesEntities2())
            {
                tb_mantenedora mantenedora = new tb_mantenedora()
                {
                    id = objeto.id,
                    id_endereco = id_end,
                    id_contato = id_cont,
                    
                    num_inscricao = objeto.num_inscricao,
                    razao_social = objeto.razao_social,
                    nome_fantasia = objeto.nome_fantasia,
                    atividade_principal = objeto.atividade_principal,
                    atividade_secundaria = objeto.atividade_secundaria,
                    
                    cnpj = objeto.cnpj,
                    inscricao_estadual = objeto.inscricao_estadual,
                    inscricao_municipal = objeto.inscricao_municipal,

                };
                context.tb_mantenedora.Attach(mantenedora);
                var entry = context.Entry(mantenedora);
                entry.State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();

            }
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
