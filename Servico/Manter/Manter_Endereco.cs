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
            entidade = new db_agesEntities2();
        }
        private db_agesEntities2 entidade;
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
        public void editar(tb_endereco objeto, int id)
        {

            using (db_agesEntities2 context = new db_agesEntities2())
            {
                tb_endereco end = new tb_endereco()
                {
                    id = id,
                    logradouro = objeto.logradouro,
                    numero = objeto.numero,
                    complemento = objeto.complemento,
                    cep= objeto.cep,
                    bairro = objeto.bairro,
                    municipio = objeto.municipio,
                    uf = objeto.uf,

                };
                context.tb_endereco.Attach(end);
                var entry = context.Entry(end);
                entry.State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();

            }
        }
       
        public tb_endereco obterEndereco(object filtro)
        {
            return entidade.tb_endereco.Where(f => f.id.Equals(filtro)).FirstOrDefault();
        }
    }
}
