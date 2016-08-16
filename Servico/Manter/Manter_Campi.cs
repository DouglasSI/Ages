﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Servico.Manter
{
    public class Manter_Campi
    {
        public Manter_Campi()
        {
            entidade = new db_agesEntities2();
        }
        private db_agesEntities2 entidade;
        public List<tb_campi> obterCampis()
        {
            List<tb_campi> campis =entidade.tb_campi.ToList();
            for(int i = 0; i < campis.Count;i++)
                campis[i].nome_fantasia = new Manter_Mantenedora().getFantasiaById(campis[i].id_mantenedora)+" - "+campis[i].nome_fantasia.ToUpper();

            return campis;
        }

        public List<tb_campi> ObterCampistwo()
        {
            List<tb_campi> campis = entidade.tb_campi.ToList();
            return campis;
        }
        public void cadastrar(tb_campi objeto)
        {
            entidade.tb_campi.Add(objeto);
            entidade.SaveChanges();
        }
        public void editar(tb_campi objeto)
        {
            entidade = new db_agesEntities2();
            entidade.tb_campi.Attach(objeto);
            var entry = entidade.Entry(objeto);
            entry.State = System.Data.Entity.EntityState.Modified;
            entidade.SaveChanges();
        }
        public tb_campi obterCampi(int filtro)
        {
            tb_campi tb_campi  = entidade.tb_campi.Where(f => f.id.Equals(filtro)).FirstOrDefault();
            return tb_campi;
        }
        public void remover(int id)
        {

            entidade.tb_campi.Remove(obterCampi(id));
            entidade.SaveChanges();
        }
    }
}
