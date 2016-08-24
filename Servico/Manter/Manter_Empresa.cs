using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Servico.Manter
{
    public class Manter_Empresa
    {
        public Manter_Empresa()
        {
            entidade = new db_agesEntities2();
        }
        private db_agesEntities2 entidade;
        public string getFantasiaById(int id)
        {
            return entidade.tb_empresa.Where(f => f.id.Equals(id) ).FirstOrDefault().nome_fantasia.ToUpper();
        }
        public List<tb_empresa> obterEmpresas()
        {
            return entidade.tb_empresa.ToList();
        }
        public void cadastrar(tb_empresa objeto)
        {
            entidade.tb_empresa.Add(objeto);
            entidade.SaveChanges();
        }
        public void editar(tb_empresa objeto, int id_end, int id_cont)
        {

            using (db_agesEntities2 context = new db_agesEntities2())
            {
                tb_empresa emp = new tb_empresa()
                {
                    id = objeto.id,
                    id_endereco = id_end,
                    id_contato = id_cont,

                    
                    razao_social = objeto.razao_social,
                    nome_fantasia = objeto.nome_fantasia,
                    atividade_principal = objeto.atividade_principal,
                    

                    cnpj = objeto.cnpj,
                    inscricao_estadual = objeto.inscricao_estadual,
                    inscricao_municipal = objeto.inscricao_municipal,

                };
                context.tb_empresa.Attach(emp);
                var entry = context.Entry(emp);
                entry.State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();

            }
        }

        public void remover(int id)
        {

            entidade.tb_empresa.Remove(obterEmpresa(id));
            entidade.SaveChanges();
        }
        public tb_empresa obterEmpresa(int filtro)
        {
            tb_empresa projeto = entidade.tb_empresa.Where(f => f.id.Equals(filtro)).FirstOrDefault();
            return projeto;
        }
    }
}
