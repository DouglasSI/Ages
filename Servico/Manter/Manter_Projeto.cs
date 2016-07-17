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
            entidade = new Entities();
        }
        private Entities entidade;
       
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
        public void editar(tb_projeto objeto)
        {
            entidade.tb_projeto.Attach(objeto);
            var entry = entidade.Entry(objeto);
            entry.State = System.Data.Entity.EntityState.Modified;

            entry.Property(e => e.id_status).IsModified = false;

            
            
            entidade.SaveChanges();
        }
        public void remover(int id)
        {

            entidade.tb_projeto.Remove(obterProjeto(id));
            entidade.SaveChanges();
        }
        public tb_projeto obterProjeto(int filtro)
        {
            tb_projeto projeto = entidade.tb_projeto.Where(f => f.id.Equals(filtro)).FirstOrDefault();
            return projeto ;
        }
    }
}
