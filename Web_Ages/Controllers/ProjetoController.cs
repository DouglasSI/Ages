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
    [Authorize]
    public class ProjetoController : Controller
    {
        // GET: Projeto
      
        public ActionResult Listar()
        {
            ViewBag.Analise = new Manter_Projeto().obterAnalises();
            return PartialView(new Servico.Manter.Manter_Projeto().obterPropostas());
        }
        public ActionResult Projetos()
        {
            ViewBag.Finalizado = new Manter_Projeto().obterFinalizados();
            ViewBag.Suspensos = new Manter_Projeto().obterSuspensos();
            return PartialView(new Manter_Projeto().obterProjetos());
        }
        // GET: Projeto/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Listar");
            }
            tb_projeto tb_projeto = new Manter_Projeto().obterProjeto((int)id);
            if (tb_projeto == null)
            {
                return HttpNotFound();
            }
            return View(tb_projeto);
        }

        // GET: Projeto/Create

        public ActionResult Create()
        {
            ViewBag.id_campi = new SelectList(new Manter_Campi().obterCampis(), "id", "nome_fantasia".ToUpper());
            ViewBag.id_status = new SelectList(new List<tb_status_projeto>(){  new Manter_Status().obterById(1)} , "id", "descricao");
            ViewBag.id_usuario = new SelectList( new List<tb_usuario>(){  new Manter_Usuario().obterUsuario(   User.Identity.Name)}, "id", "nome");
            return View();
        }

        // POST: Projeto/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include = "id,id_usuario,id_status,id_campi,titulo,anotacao,valor_estimado")] tb_projeto tb_projeto)
        {
            if (ModelState.IsValid)
            {
                new Manter_Projeto().cadastrar(tb_projeto);

                return RedirectToAction("Listar");
            }
            ViewBag.id_campi = new SelectList(new Manter_Campi().obterCampis(), "id", "nome_fantasia".ToUpper());
            ViewBag.id_status = new SelectList(new List<tb_status_projeto>() { new Manter_Status().obterById(1) }, "id_status", "descricao");
            ViewBag.id_usuario = new SelectList(new List<tb_usuario>() { new Manter_Usuario().obterUsuario(User.Identity.Name) }, "id_usuario", "nome");
            return View(tb_projeto);
        }

        // GET: Projeto/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Listar");
            }
            tb_projeto tb_projeto = new Manter_Projeto().obterProjeto((int)id);
            if (tb_projeto == null)
            {
                return RedirectToAction("Listar");
            }
            ViewBag.id_campi = new SelectList(new Manter_Campi().obterCampis(), "id", "nome_fantasia".ToUpper());
            ViewBag.id_status = new SelectList(new List<tb_status_projeto>() { new Manter_Status().obterById(tb_projeto.id_status) }, "id_status", "descricao");
            ViewBag.id_usuario = new SelectList(new List<tb_usuario>() { new Manter_Usuario().obterUsuario(User.Identity.Name) }, "id_usuario", "nome");
            return View(tb_projeto);
        }

        // POST: Projeto/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit([Bind(Include = "id,id_usuario,id_status,id_campi,titulo,anotacao,valor_estimado")] tb_projeto tb_projeto)
        {
            if (ModelState.IsValid)
            {
                tb_projeto.id_usuario = new Manter_Usuario().obterUsuario(User.Identity.Name).id;
                new Manter_Projeto().editar(tb_projeto);
                return RedirectToAction("Listar");
            }
            ViewBag.id_campi = new SelectList(new Manter_Campi().obterCampis(), "id", "nome_fantasia".ToUpper());
            ViewBag.id_status = new SelectList(new List<tb_status_projeto>() { new Manter_Status().obterById(tb_projeto.id_status) }, "id_status", "descricao");
            ViewBag.id_usuario = new SelectList(new List<tb_usuario>() { new Manter_Usuario().obterUsuario(User.Identity.Name) }, "id_usuario", "nome");
            return View(tb_projeto);
        }

        // GET: Projeto/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Listar");
            }
            tb_projeto tb_projeto = new Manter_Projeto().obterProjeto((int)id);
            if (tb_projeto == null)
            {
                return RedirectToAction("Listar");
            }
            return View(tb_projeto);
        }

        // POST: Projeto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            new Manter_Projeto().remover(id);
            return RedirectToAction("Listar");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
//
            }
            base.Dispose(disposing);
        }
    }
}
