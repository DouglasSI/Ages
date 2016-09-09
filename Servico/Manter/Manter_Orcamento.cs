using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Servico.Manter
{
    public class Manter_Orcamento
    {

        public Manter_Orcamento()
        {
            entidade = new db_agesEntities2();
        }
        private db_agesEntities2 entidade;

        public List<tb_orcamento> obterOrcamentos(int id_projeto)
        {
            List<tb_orcamento> retur =entidade.tb_orcamento.ToList().Where(f => f.id_projeto.Equals(id_projeto)).ToList();
            return retur;
        }

        public List<tb_orcamento> obterOrcamentosPorEmpresa(int id_empresa)
        {
            List<tb_orcamento> retur = entidade.tb_orcamento.ToList().Where(f => f.id_empresa.Equals(id_empresa)).ToList();
            return retur;
        }
        public List<tb_orcamento> obterOrcamentosAprovados(int id_projeto)
        {
            List<tb_orcamento> retur = entidade.tb_orcamento.ToList().Where(f => f.id_projeto.Equals(id_projeto) && f.id_status.Equals(2)).ToList();
            return retur;
        }

        public void cadastrar(tb_orcamento objeto)
        {
            if (objeto.anotacao == null)
            { objeto.anotacao = "-"; }
            entidade.tb_orcamento.Add(objeto);
            //entidade.SaveChangesAsync();
            entidade.SaveChanges();
        }
        public void editarStatus(tb_orcamento orcamento,  int id_status)
        {
            if (orcamento.anotacao == null)
            { orcamento.anotacao = "-"; }
            using (db_agesEntities2 context = new db_agesEntities2())
            {
                tb_orcamento orc = new tb_orcamento()
                {
                    id = orcamento.id,
                    id_status = id_status,
                    valor = orcamento.valor,
                    anotacao = orcamento.anotacao,
                    id_empresa = orcamento.id_empresa,
                    id_projeto = orcamento.id_projeto,
                    id_usuario = orcamento.id_usuario
                };
                context.tb_orcamento.Attach(orc);
                var entry = context.Entry(orc);
                entry.State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();

            }

        }
        public void editar(tb_orcamento objeto)
        {
            if (objeto.anotacao == null)
            { objeto.anotacao = "-"; }
            using (db_agesEntities2 context = new db_agesEntities2())
            {
                tb_orcamento orc = new tb_orcamento()
                {
                    id = objeto.id,
                    id_status = objeto.id_status,
                    valor = objeto.valor,
                    anotacao = objeto.anotacao,
                    id_empresa = objeto.id_empresa,
                    id_projeto = objeto.id_projeto,
                    id_usuario = objeto.id_usuario
                };
                context.tb_orcamento.Attach(orc);
                var entry = context.Entry(orc);
                entry.State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();

            }

            
            
        }
        public void remover(int? id)
        {
            tb_orcamento orcamento = obterOrcamento((int)id);
            entidade.tb_anexo.RemoveRange(orcamento.tb_anexo);
            entidade.tb_orcamento.Remove(orcamento);
            entidade.SaveChanges();
        }
        public tb_orcamento obterOrcamento(int filtro)
        {
            tb_orcamento projeto = entidade.tb_orcamento.Where(f => f.id.Equals(filtro)).FirstOrDefault();
            return projeto;
        }

    }
}

