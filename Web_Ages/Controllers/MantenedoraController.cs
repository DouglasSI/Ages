﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Model;
using Servico.Manter;

namespace Web_Ages.Controllers
{
    [Authorize]
    public class MantenedoraController : Controller
    {
        // GET: Mantenedora
        public ActionResult Index()
        {
            return PartialView(new Manter_Mantenedora().obterMantenedoras());
        }

        // GET: Mantenedora/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            tb_mantenedora tb_mantenedora = new Manter_Mantenedora().obterMantenedora((int)id);
            if (tb_mantenedora == null)
            {
                return RedirectToAction("Index");
            }

            return View(tb_mantenedora);
        }

        // GET: Mantenedora/Create
        public ActionResult Create()
        {
            // tem que ter cadastro de endereco e contato nesta tablea
            return View();
        }

        // POST: Mantenedora/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,id_endereco,id_contato,num_inscricao,razao_social,nome_fantasia,atividade_principal,atividade_secundaria,cnpj,inscricao_estadual,inscricao_municipal,tb_contato,tb_endereco")]  tb_mantenedora tb_mantenedora)
        {
                if (ModelState.IsValid)
            {
                    tb_mantenedora.id_contato = new Manter_Contato().cadastrar(tb_mantenedora.tb_contato).id;
                    tb_mantenedora.id_endereco = new Manter_Endereco().cadastrar(tb_mantenedora.tb_endereco).id;
                        
                new Manter_Mantenedora().cadastrar(tb_mantenedora);

                return RedirectToAction("Index");
            }

            return View(tb_mantenedora);
        }

        // GET: Mantenedora/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            tb_mantenedora tb_mantenedora = new Manter_Mantenedora().obterMantenedora((int)id);
            if (tb_mantenedora == null)
            {
                return RedirectToAction("Index");
            }
          
            return View(tb_mantenedora);
        }

        // POST: Mantenedora/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
       
        public ActionResult Edit([Bind(Include = "id,id_endereco,id_contato,num_inscricao,razao_social,nome_fantasia,atividade_principal,atividade_secundaria,cnpj,inscricao_estadual,inscricao_municipal,tb_contato,tb_endereco")] tb_mantenedora tb_mantenedora)
        {
            if (ModelState.IsValid)
            {
                tb_mantenedora.id_contato = new Manter_Contato().cadastrar(tb_mantenedora.tb_contato).id;
                tb_mantenedora.id_endereco = new Manter_Endereco().cadastrar(tb_mantenedora.tb_endereco).id;
                new Manter_Mantenedora().editar(tb_mantenedora);
                return RedirectToAction("Index");
            }
            return View(tb_mantenedora);
        }

        // GET: Mantenedora/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            tb_mantenedora tb_mantenedora = new Manter_Mantenedora().obterMantenedora((int)id);
            if (tb_mantenedora == null)
            {
                return RedirectToAction("Index");
            }
            return View(tb_mantenedora);
        }

        // POST: Mantenedora/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            new Manter_Mantenedora().remover(id);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
            base.Dispose(disposing);
        }
    }
}
