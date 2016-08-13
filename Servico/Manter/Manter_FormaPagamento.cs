using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servico.Manter
{
    public class Manter_FormaPagamento
    {
        public Manter_FormaPagamento()
        {
            entidade = new db_agesEntities2();
        }
        private db_agesEntities2 entidade;
        public List<tb_forma_pagamento> obterFormasPag()
        {
            return entidade.tb_forma_pagamento.ToList();
        }
        public void cadastrar(tb_forma_pagamento objeto)
        {
            entidade.tb_forma_pagamento.Add(objeto);
            entidade.SaveChanges();
        }
        public void editar(tb_forma_pagamento objeto)
        {
            entidade = new db_agesEntities2();
            entidade.tb_forma_pagamento.Attach(objeto);
            var entry = entidade.Entry(objeto);
            entry.State = System.Data.Entity.EntityState.Modified;

            entidade.SaveChanges();
        }
        public void remover(int id)
        {

            entidade.tb_forma_pagamento.Remove(obterFormaPagamento(id));
            entidade.SaveChanges();
        }
        public tb_forma_pagamento obterFormaPagamento(int filtro)
        {
            tb_forma_pagamento projeto = entidade.tb_forma_pagamento.Where(f => f.id.Equals(filtro)).FirstOrDefault();
            return projeto;
        }
    }
}
