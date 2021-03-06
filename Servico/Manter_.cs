﻿using System;
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
        private db_agesEntities2 entidade;
        public Manter_()
        {
            //entidade = new db_agesEntities1();
        }
        public tb_anexo getAnexo(int id)
        {
            using (db_agesEntities2 contexto = new db_agesEntities2())
            {
                return contexto.tb_anexo.Where(r => r.id.Equals( id)).First();
            }

        }
        public void removerAnexoProjeto(int id)
        {
            using (db_agesEntities2 context = new db_agesEntities2())
            {
                context.tb_anexo.Remove(context.tb_anexo.Find(id));
                context.SaveChanges();
            }
        }
        public List<tb_anexo> getAnexos(int id , tipo tipo)
        {

            using (db_agesEntities2 contexto = new db_agesEntities2())
            {

                switch (tipo)
                {
                    case tipo.orcamento:
                        {
                            List<tb_anexo> tb_anexos  = contexto.tb_anexo.Where(s => s.tb_orcamento.Any(c => c.id == id)).ToList();

                            return tb_anexos;
                        } break;
                    case tipo.compra:
                        {
                            
                            

                            
                        } break;
                    case tipo.fatura:
                        {
                            
                        } break;
                    case tipo.empresa:
                        {
                            
                        } break;
                    
                    case tipo.projeto:
                        {
                            
                        } break;
                }
                return null;
            }
            return null;
        }
        public void PersistirGrupo(tb_orcamento grupo)
        {
            if (grupo.anotacao == null)
            {grupo.anotacao = "-";}
            
            using (db_agesEntities2 context = new db_agesEntities2())
            {
                //new Manter_Orcamento(context).cadastrar(grupo);
                tb_orcamento orc =

                context.tb_orcamento.Add(new tb_orcamento()
                {
                    id = grupo.id,
                    anotacao = grupo.anotacao,
                    id_empresa = grupo.id_empresa,
                    id_projeto = grupo.id_projeto,
                    id_status = grupo.id_status,
                    id_usuario = grupo.id_usuario,
                    valor = grupo.valor
                });
                context.SaveChanges();
                foreach (tb_orcamento_servico os in grupo.tb_orcamento_servico)
                {
                    context.tb_orcamento_servico.Add(new tb_orcamento_servico()
                    {
                        id_orcamento = orc.id,
                        id_servico = os.id_servico,
                        anotacao = os.anotacao,
                        valor_servico = os.valor_servico
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
                foreach (tb_anexo ff in grupo.tb_anexo)
                {
                    PersistirAnexo(ff, tipo.orcamento, orc.id);
                }
                tb_projeto p = context.tb_projeto.Where(f => f.id.Equals(orc.id_projeto)).FirstOrDefault();
                p.id_status = 2;

                context.tb_projeto.Attach(p);

                var entry = context.Entry(p);
                entry.State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }
        }
        public void PersistirFatura(tb_fatura objeto)
        {
            using (db_agesEntities2 enty = new db_agesEntities2())
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

                foreach (tb_anexo ff in objeto.tb_anexo)
                {
                    PersistirAnexo(ff, tipo.fatura, fatura.id);
                }
            }
        }
        public int PersistirCompra(tb_compra compra)
        {
            using (db_agesEntities2 context = new db_agesEntities2())
            {
                tb_fatura fat = context.tb_fatura.Where(f => f.id.Equals(compra.id_fatura)).FirstOrDefault();
                if (fat.valor_pendente < compra.valor)
                    return -1;
                context.tb_compra.Add(compra);
                context.SaveChanges();
                fat.valor_pendente -= compra.valor;
                context.tb_fatura.Attach(fat);
                var entry = context.Entry(fat);
                entry.State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
                return compra.id;
            }
        }
        public enum tipo { fatura, orcamento, projeto, compra, empresa };
        public tb_anexo PersistirAnexo(tb_anexo anexo, tipo N, int id)
        {

            using (db_agesEntities2 contexts = new db_agesEntities2())
            {
                tb_anexo anex = new tb_anexo()
                {
                    id_usuario = anexo.id_usuario,
                    anotacao = "observação",
                    titulo = anexo.titulo,
                    data_cadastro = anexo.data_cadastro,
                    tipo = anexo.tipo,
                    tamanho = anexo.tamanho,
                    caminho = anexo.caminho,
                    nome_arquivo = anexo.nome_arquivo,
                    url = "url",
                };
                switch (N)
                {
                    case tipo.compra:
                        {
                            anex.tb_compra.Add(contexts.tb_compra.Find(id));
                        } break;
                    case tipo.fatura:
                        {
                            anex.tb_fatura.Add(contexts.tb_fatura.Find(id));
                        } break;
                    case tipo.empresa:
                        {
                            anex.tb_empresa.Add(contexts.tb_empresa.Find(id));
                        } break;
                    case tipo.orcamento:
                        {
                            anex.tb_orcamento.Add(contexts.tb_orcamento.Find(id));
                        } break;
                    case tipo.projeto:
                        {
                            anex.tb_projeto.Add(contexts.tb_projeto.Find(id));
                        } break;
                }
                contexts.tb_anexo.Add(anex);
                contexts.SaveChanges();
                return contexts.tb_anexo.FirstOrDefault();
            };

        }
        public Manter_Orcamento_Servico M_Orcamento_Servico()
        {

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
