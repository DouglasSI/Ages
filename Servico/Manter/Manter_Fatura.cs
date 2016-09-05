using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using System.Data.Entity;

namespace Servico.Manter
{
    public class Manter_Fatura
    {
        public Manter_Fatura()
        {
            entidade = new db_agesEntities2();
        }
        private db_agesEntities2 entidade;
        
        public void autorizar(tb_fatura fat)
        {

            using (db_agesEntities2 context = new db_agesEntities2())
            {
                tb_fatura fatura = new tb_fatura()
                {
                    id= fat.id,
                    id_forma_pagamento=fat.id_forma_pagamento,
                    id_orcamento = fat.id_orcamento,
                    id_usuario = fat.id_usuario,
                    is_aditivo = fat.is_aditivo,
                    agencia = fat.agencia,
                    banco = fat.banco,
                    conta = fat.conta,
                    anotacao = fat.anotacao,
                    titulo = fat.titulo,
                    valor_inicial = fat.valor_inicial,
                    valor_pendente = fat.valor_pendente,
                    data_cadastro = fat.data_cadastro,
                    data_pagamento = fat.data_pagamento,
                    data_prevista = fat.data_prevista,
                    autorizado = fat.autorizado,
                    encerrado = fat.encerrado
                   
                };
                context.tb_fatura.Attach(fatura);
                var entry = context.Entry(fatura);
                entry.State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();

            }

        }


        public List<tb_fatura> ObterFaturasporOrcamento(int id)
        {
            List<tb_fatura> faturas = entidade.tb_fatura.ToList().Where(f => f.id_orcamento.Equals(id)).ToList();
            return faturas;
        }
        public tb_fatura ObterFatura (int id)
        {
            tb_fatura tb_fatura = entidade.tb_fatura.Where(f => f.id.Equals(id)).FirstOrDefault();
            return tb_fatura;
        }
        public tb_fatura ObterFaturaporOrcamento(int? id)
        {
            tb_fatura fatura = entidade.tb_fatura.Where(f => f.id_orcamento.Equals((int)id)).FirstOrDefault();
            return fatura;
        }
        public void remover(int? id)
        {

            entidade.tb_fatura.Remove(ObterFaturaporOrcamento((int)id));
            entidade.SaveChanges();
        }
    }
}
