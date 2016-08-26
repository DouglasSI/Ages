using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servico.Manter
{
    public class Manter_Projeto
    {
        public Manter_Projeto()
        {
            entidade = new db_agesEntities2();
        }
        private db_agesEntities2 entidade;
       
        public List<tb_projeto> obterPropostas()
        {
            return entidade.tb_projeto.ToList()
                .Where(f => f.id_status.Equals(1)).ToList();
        }
        public List<tb_projeto> obterAnalises()
        {
            return entidade.tb_projeto.ToList()
                .Where(f => f.id_status.Equals(2)).ToList();
        }
        public List<tb_projeto> obterProjetos()
        {
            return entidade.tb_projeto.ToList()
                .Where(f => f.id_status.Equals(3)).ToList();
        }
        public List<tb_projeto> obterFinalizados()
        {
            return entidade.tb_projeto.ToList()
                .Where(f => f.id_status.Equals(4)).ToList();
        }
        public List<tb_projeto> obterSuspensos()
        {
            return entidade.tb_projeto.ToList()
                .Where(f => f.id_status.Equals(5)).ToList();
        }
        public void cadastrar(tb_projeto objeto)
        {
            entidade.tb_projeto.Add(objeto);
            entidade.SaveChanges();
        }
        public void editarStatus(tb_projeto proj, int id_status)
        {

            using (db_agesEntities2 context = new db_agesEntities2())
            {
                tb_projeto pr = new tb_projeto()
                {
                    id = proj.id,
                    id_status = id_status,
                    id_campi = proj.id_campi,
                    id_usuario = proj.id_usuario,
                    titulo = proj.titulo,
                    valor_estimado = proj.valor_estimado,
                    anotacao = proj.anotacao,
                   
                };
                context.tb_projeto.Attach(pr);
                var entry = context.Entry(pr);
                entry.State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();

            }

        }
     
        public void editar(tb_projeto objeto)
        {
            entidade = new db_agesEntities2();
            entidade.tb_projeto.Attach(objeto);
            var entry = entidade.Entry(objeto);
            entry.State = System.Data.Entity.EntityState.Modified;
            entidade.SaveChanges();
        }
        public void editarStatus(int id_projeto, int id_status)
        {
            tb_projeto p = obterProjeto(id_projeto);
            p.id_status = id_status;

            entidade = new db_agesEntities2();
            entidade.tb_projeto.Attach(p);
            var entry = entidade.Entry(p);
            entry.State = System.Data.Entity.EntityState.Modified;
            entidade.SaveChanges();
        }
        public void remover(int id)
        {
            tb_projeto proposta = obterProjeto(id);
            entidade.tb_anexo.RemoveRange(proposta.tb_anexo);
            entidade.tb_orcamento.RemoveRange(proposta.tb_orcamento);
            entidade.tb_projeto.Remove(proposta);

            entidade.SaveChanges();
        }
        public tb_projeto obterProjeto(int filtro)
        {
            tb_projeto projeto = entidade.tb_projeto.Where(f => f.id.Equals(filtro)).FirstOrDefault();
            return projeto ;
        }
    }
}
