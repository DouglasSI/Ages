using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Servico.Manter;

namespace Servico
{
    public class Manter_
    {
        private Entities entidade;
        public Manter_()
        {
            //entidade = new Entities();
        }
        public void PersistirGrupo( tb_orcamento grupo)
        {
            using (Entities context = new Entities())
              {
                  //new Manter_Orcamento(context).cadastrar(grupo);
                tb_orcamento orc =  
                
                context.tb_orcamento.Add(new tb_orcamento() {
                    id=  grupo.id,
                    anotacao = grupo.anotacao,
                     id_empresa = grupo.id_empresa,
                      id_projeto = grupo.id_projeto,
                       id_status = grupo.id_status,
                        id_usuario =  grupo.id_usuario,
                         valor = grupo.valor
                  });
                context.SaveChanges();
                  foreach (tb_orcamento_servico os in grupo.tb_orcamento_servico)
                  {
                      context.tb_orcamento_servico.Add(new tb_orcamento_servico() {
                       id_orcamento = orc.id,
                        id_servico = os.id_servico,
                         anotacao = os.anotacao,
                          valor = os.valor
                      });
                      context.SaveChanges();
                  }
                  
                  foreach (tb_fatura ff in grupo.tb_fatura)
                  {
                      context.tb_fatura.Add(new tb_fatura()
                      {
                          id_forma_pagamento = ff.id_forma_pagamento,
                          id_usuario = grupo.id_usuario,
                          id_orcamento = orc.id,
                          is_aditivo = false,
                          titulo = ff.titulo,
                          valor_inicial = ff.valor_inicial,
                          valor_pendente = ff.valor_inicial,
                          data_cadastro = DateTime.Now,
                          agencia = ff.agencia,
                          banco = ff.banco,
                          anotacao = ff.anotacao,
                          conta = ff.conta,
                          data_pagamento = null,
                          data_prevista = ff.data_prevista
                      });
                      context.SaveChanges();
                  }
                tb_projeto p  = context.tb_projeto.Where(f => f.id.Equals(orc.id_projeto)).FirstOrDefault();
                p.id_status = 2;

                context.tb_projeto.Attach(p);

                var entry = context.Entry(p);
                entry.State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
              }
        }
        public void PersistirFatura(tb_fatura objeto)
        {
            using (Entities enty = new Entities())
            {
                tb_fatura fatura = new tb_fatura()
                {
                    agencia = objeto.agencia,
                    anotacao = objeto.anotacao,
                    autorizado = false,
                    banco = objeto.banco,
                    id_forma_pagamento = objeto.id_forma_pagamento,
                    id_orcamento = objeto.id_orcamento,
                    valor_inicial = objeto.valor_inicial,
                    valor_pendente = objeto.valor_inicial,
                    titulo = objeto.titulo,
                    is_aditivo = objeto.is_aditivo,
                    id_usuario = objeto.id_usuario,
                    data_prevista = objeto.data_prevista,
                    data_cadastro = DateTime.Now,
                    conta = objeto.conta
                };

                enty.tb_fatura.Add(fatura);
                enty.SaveChanges();


            }
        }
        public void PersistirCompra( tb_compra compra)
        {
            using (Entities context = new Entities())
            {

                context.tb_compra.Add(new tb_compra() {
                anotacao = compra.anotacao,
                data_compra = compra.data_compra,
                 id_fatura = compra.id_fatura,
                 valor = compra.valor
                });
                context.SaveChanges();
                tb_fatura fat = context.tb_fatura.Where(f => f.id.Equals(compra.id_fatura)).FirstOrDefault();
                 fat . valor_pendente = compra.tb_fatura.valor_pendente;

                context.tb_fatura.Attach(fat);
                var entry = context.Entry(fat);
                entry.State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();



            }
        }
        public Manter_Orcamento_Servico M_Orcamento_Servico(){

            return new Manter_Orcamento_Servico();

        }
        public Manter_Orcamento M_Orcamento()
        {

            return new Manter_Orcamento();

        }
        public void Save()
        {
            entidade.SaveChanges();

        }
    }
}
