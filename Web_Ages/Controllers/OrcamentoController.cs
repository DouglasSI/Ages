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
    public class OrcamentoController : Controller
    {
        private Entities db = new Entities();

        // GET: Orcamento
        public ActionResult Index()
        {
            var tb_orcamento = db.tb_orcamento.Include(t => t.tb_empresa).Include(t => t.tb_projeto).Include(t => t.tb_status_orcamento).Include(t => t.tb_usuario);
            return View(tb_orcamento.ToList());
        }

        // GET: Orcamento/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {

                return RedirectToAction("Projetos");
            }
            ViewBag.Orcamento = new Manter_Orcamento().obterOrcamentos();

            return View();
        }

        // GET: Orcamento/Create
        public ActionResult Create(int? id_projeto)
        {
            ViewBag.id_empresa = new SelectList(new Manter_Empresa().obterEmpresas(), "id", "nome_fantasia");
            ViewBag.id_servico = new SelectList(new Manter_Servico().obterServicos(), "id", "titulo");

             
             ViewBag.id_projeto = new Manter_Projeto().obterProjeto((int)id_projeto);
             ViewBag.id_campi = new Manter_Campi().obterCampis().Where(x => x.id == ((tb_projeto)ViewBag.id_projeto).id_campi).FirstOrDefault();
             ViewBag.id_status = new Manter_Status().obterByIdOrcamento(1);
             ViewBag.id_usuario = new Manter_Usuario().obterUsuario(User.Identity.Name);
            return View();
        }
        public void RemoverServico(int id_servico)
        {
            foreach (tb_orcamento_servico os in ((List<tb_orcamento_servico>)ViewBag.Servicos))
                if (os.id_servico == id_servico)
                    ((List<tb_orcamento_servico>)ViewBag.Servicos).Remove(os);
        }
        // POST: Orcamento/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Model.Super.SuperOrcamento tb_orcamento)
        {
            ViewBag.id_empresa = new SelectList(new Manter_Empresa().obterEmpresas(), "id", "nome_fantasia");
            ViewBag.id_servico = new SelectList(new Manter_Servico().obterServicos(), "id", "titulo");


            ViewBag.id_projeto = new Manter_Projeto().obterProjeto(tb_orcamento.orcamento.id_projeto);
            ViewBag.id_campi = new Manter_Campi().obterCampis().Where(x => x.id == ((tb_projeto)ViewBag.id_projeto).id_campi).FirstOrDefault();
            ViewBag.id_status = new Manter_Status().obterByIdOrcamento(1);
            ViewBag.id_usuario = new Manter_Usuario().obterUsuario(User.Identity.Name);
            if (Request.Form["bt_submit_1"] != null)
            {
                if (ModelState.IsValid)
                {
                    db.tb_orcamento.Add(tb_orcamento.orcamento);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            else if (Request.Form["bt_submit_2"] != null)
            {
                tb_orcamento.orcamento.tb_orcamento_servico.Add(new tb_orcamento_servico() 
                { id_servico = tb_orcamento.servico.id, data_cadastro = DateTime.Now, valor = tb_orcamento.servico.valor, anotacao = tb_orcamento.servico.anotacao });
                return View(tb_orcamento);
            }
            return View(tb_orcamento);
            
        }

        // GET: Orcamento/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_orcamento tb_orcamento = db.tb_orcamento.Find(id);
            if (tb_orcamento == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_empresa = new SelectList(db.tb_empresa, "id", "razao_social", tb_orcamento.id_empresa);
            ViewBag.id_projeto = new SelectList(db.tb_projeto, "id", "titulo", tb_orcamento.id_projeto);
            ViewBag.id_status = new SelectList(db.tb_status_orcamento, "id", "descricao", tb_orcamento.id_status);
            ViewBag.id_usuario = new SelectList(db.tb_usuario, "id", "nome", tb_orcamento.id_usuario);
            return View(tb_orcamento);
        }

        // POST: Orcamento/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,id_usuario,id_status,id_empresa,id_projeto,valor,anotacao")] tb_orcamento tb_orcamento)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tb_orcamento).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_empresa = new SelectList(db.tb_empresa, "id", "razao_social", tb_orcamento.id_empresa);
            ViewBag.id_projeto = new SelectList(db.tb_projeto, "id", "titulo", tb_orcamento.id_projeto);
            ViewBag.id_status = new SelectList(db.tb_status_orcamento, "id", "descricao", tb_orcamento.id_status);
            ViewBag.id_usuario = new SelectList(db.tb_usuario, "id", "nome", tb_orcamento.id_usuario);
            return View(tb_orcamento);
        }

        // GET: Orcamento/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_orcamento tb_orcamento = db.tb_orcamento.Find(id);
            if (tb_orcamento == null)
            {
                return HttpNotFound();
            }
            return View(tb_orcamento);
        }

        // POST: Orcamento/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tb_orcamento tb_orcamento = db.tb_orcamento.Find(id);
            db.tb_orcamento.Remove(tb_orcamento);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
