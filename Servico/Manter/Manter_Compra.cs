using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Servico.Manter
{
    public class Manter_Compra
    {
        public Manter_Compra()
        {
            entidade = new db_agesEntities2();
        }
        private db_agesEntities2 entidade;
        public List<tb_compra> obterCompras()
        {
            return entidade.tb_compra.ToList();
        }
        public void cadastrar(tb_compra objeto)
        {
            entidade.tb_compra.Add(objeto);
        }
        public void editar(tb_compra objeto)
        {
            using (db_agesEntities2 context = new db_agesEntities2())
            {
                tb_compra comp = new tb_compra();
                {
                    comp.id = objeto.id;
                    comp.id_empresa = objeto.id_empresa;
                    comp.id_fatura = objeto.id_fatura;
                    comp.anotacao = objeto.anotacao;
                    comp.autorizado = objeto.autorizado;
                    comp.data_autorizacao = objeto.data_autorizacao;
                    comp.data_compra = objeto.data_compra;
                    comp.data_pagamento = objeto.data_pagamento;
                    comp.encerrado = objeto.encerrado;
                    comp.valor = objeto.valor;

                };
                context.tb_compra.Attach(comp);
                var entry = context.Entry(comp);
                entry.State = System.Data.Entity.EntityState.Modified;
                
                context.SaveChanges();
            };
          
          
           
        }
        public tb_compra obterCompra(object filtro)
        {
            return entidade.tb_compra.Where(f => f.id.Equals(filtro)).FirstOrDefault();
        }
        public tb_compra obterCompraInt(int id)
        {
            tb_compra compra = entidade.tb_compra.Where(f => f.id.Equals(id)).FirstOrDefault();
            return compra;
        }


    }
}
