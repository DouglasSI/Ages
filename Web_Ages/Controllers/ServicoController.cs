using System;
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
    public class ServicoController : Controller
    {
        [Authorize]
        // GET: Servico
        public ActionResult Index()
        {
            return PartialView(new Manter_Servico().obterServicos());
        }

        // GET: Servico/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            tb_servico tb_servico = new Manter_Servico().obterServico((int)id);
            if (tb_servico == null)
            {
                return RedirectToAction("Index");
            }
            return View(tb_servico);
        }

        // GET: Servico/Create
        public ActionResult Create()
        {
            ViewBag.id_usuario = new SelectList(new List<tb_usuario>() { new Manter_Usuario().obterUsuario(User.Identity.Name) }, "id", "nome");
            return View();
        }

        // POST: Servico/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        
        public ActionResult Create([Bind(Include = "id,id_usuario,valor,titulo,anotacao,data_cadastro")] tb_servico tb_servico)
        {
            if (ModelState.IsValid)
            {
                new Manter_Servico().cadastrar(tb_servico);

                return RedirectToAction("Index");
            }
            ViewBag.id_usuario = new SelectList(new List<tb_usuario>() { new Manter_Usuario().obterUsuario(User.Identity.Name) }, "id_usuario", "nome");
            return View(tb_servico);
        }

        // GET: Servico/Edit/5
        public ActionResult Edit(int? id)
        {

            if (id == null)
            {
                return RedirectToAction("Index");
            }
            tb_servico tb_servico = new Manter_Servico().obterServico((int)id);
            if (tb_servico == null)
            {
                return RedirectToAction("Index");
            }

            ViewBag.id_usuario = ( new SelectList(new List<tb_usuario>() { new Manter_Usuario().obterUsuario(User.Identity.Name) }, "id", "nome"));
            
            return View(tb_servico);
        }

        // POST: Servico/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        
        public ActionResult Edit([Bind(Include = "id,id_usuario,valor,titulo,anotacao")] tb_servico tb_servico)
        {
            tb_servico.data_cadastro = DateTime.Now;
            if (ModelState.IsValid)
            {
                new Manter_Servico().editar(tb_servico);
                return RedirectToAction("Index");
            }
            ViewBag.id_usuario = new SelectList(new List<tb_usuario>() { new Manter_Usuario().obterUsuario(User.Identity.Name) }, "id_usuario", "nome");
            return View(tb_servico);
        }

        // GET: Servico/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            tb_servico tb_servico = new Manter_Servico().obterServico((int)id);
            if (tb_servico == null)
            {
                return RedirectToAction("Index");
            }
            return View(tb_servico);
        }

        // POST: Servico/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            new Manter_Servico().remover(id);
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
