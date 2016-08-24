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
    public class CampiController : Controller
    {
        // GET: Campi
        public ActionResult Index()
        {
            return PartialView(new Manter_Campi().ObterCampistwo());
        }

        // GET: Campi/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            tb_campi tb_campi = new Manter_Campi().obterCampi((int)id);
            if (tb_campi == null)
            {
                return RedirectToAction("Index");
            }

            return View(tb_campi);
        }

        // GET: Campi/Create
        [Authorize(Roles = "INFRA,DIRETOR-INFRA")]
        public ActionResult Create()
        {
            // tem que ter cadastro de endereco e contato nesta tablea
            ViewBag.id_mantenedora = new SelectList(new Manter_Mantenedora().obterMantenedoras(), "id", "nome_fantasia");
            return View();
        }

        // POST: Campi/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "INFRA,DIRETOR-INFRA")]
        public ActionResult Create([Bind(Include = "id,id_endereco,id_contato,id_mantenedora,num_inscricao,razao_social,nome_fantasia,atividade_principal,atividade_secundaria,natureza_juridica,cnpj,inscricao_estadual,inscricao_municipal,tb_contato,tb_endereco")] tb_campi tb_campi)
        {
            if (ModelState.IsValid)

            {
                tb_campi.id_contato = new Manter_Contato().cadastrar(tb_campi.tb_contato).id;
                tb_campi.id_endereco = new Manter_Endereco().cadastrar(tb_campi.tb_endereco).id;
                new Manter_Campi().cadastrar(tb_campi);

                return RedirectToAction("Index");
            }
            ViewBag.id_mantenedora = new SelectList(new Manter_Mantenedora().obterMantenedoras(), "id", "nome_fantasia");
            return View(tb_campi);
        }

        // GET: Campi/Edit/5
        [Authorize(Roles = "INFRA,DIRETOR-INFRA")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            tb_campi tb_campi = new Manter_Campi().obterCampi((int)id);
            if (tb_campi == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.id_mantenedora = new SelectList(new Manter_Mantenedora().obterMantenedoras(), "id", "nome_fantasia", tb_campi.id_mantenedora);
            return View(tb_campi);
        }

        // POST: Campi/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "INFRA,DIRETOR-INFRA")]
        public ActionResult Edit([Bind(Include = "id,id_endereco,id_contato,id_mantenedora,num_inscricao,razao_social,nome_fantasia,atividade_principal,atividade_secundaria,natureza_juridica,cnpj,inscricao_estadual,inscricao_municipal,tb_contato,tb_endereco")] tb_campi tb_campi)
        {
            tb_campi  tb_campitwo = new Manter_Campi().obterCampi((int)tb_campi.id);
            if (ModelState.IsValid)
            {

                new Manter_Contato().editar(tb_campi.tb_contato,tb_campitwo.id_contato);
                new Manter_Endereco().editar(tb_campi.tb_endereco, tb_campitwo.id_endereco);
                
                new Manter_Campi().editar(tb_campi,tb_campitwo.id_endereco,tb_campitwo.id_contato);
                return RedirectToAction("Index");
            }
            

            ViewBag.id_mantenedora = new SelectList(new Manter_Mantenedora().obterMantenedoras(), "id", "nome_fantasia", tb_campi.id_mantenedora);
            return View(tb_campi);
        }

        // GET: Campi/Delete/5
        [Authorize(Roles = "INFRA,DIRETOR-INFRA")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            tb_campi tb_campi = new Manter_Campi().obterCampi((int)id);
            if (tb_campi == null)
            {
                return RedirectToAction("Index");
            }

            return View(tb_campi);
        }

        // POST: Campi/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "INFRA,DIRETOR-INFRA")]
        public ActionResult DeleteConfirmed(int id)
        {
            new Manter_Campi().remover(id);
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
