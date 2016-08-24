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
    public class EmpresaController : Controller
    {
        // GET: Empresa
        public ActionResult Index()
        {
            return PartialView(new Manter_Empresa().obterEmpresas());
        }

        // GET: Empresa/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            tb_empresa tb_empresa = new Manter_Empresa().obterEmpresa((int)id);
            if (tb_empresa == null)
            {
                return RedirectToAction("Index");
            }
            return View(tb_empresa);
        }

        // GET: Empresa/Create
        [Authorize(Roles = "INFRA,DIRETOR-INFRA")]
        public ActionResult Create()
        {
            
            return View();
        }

        // POST: Empresa/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "INFRA,DIRETOR-INFRA")]
        public ActionResult Create([Bind(Include = "id,id_endereco,id_contato,razao_social,nome_fantasia,cnpj,inscricao_estadual,inscricao_municipal,atividade_principal,tb_contato,tb_endereco")] tb_empresa tb_empresa)
        {
            if (ModelState.IsValid)
            {
                tb_empresa.id_contato = new Manter_Contato().cadastrar(tb_empresa.tb_contato).id;
                tb_empresa.id_endereco = new Manter_Endereco().cadastrar(tb_empresa.tb_endereco).id;
                new Manter_Empresa().cadastrar(tb_empresa);

                return RedirectToAction("Index");
            }
            return View(tb_empresa);
        }

        // GET: Empresa/Edit/5
        [Authorize(Roles = "INFRA,DIRETOR-INFRA")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            tb_empresa tb_empresa = new Manter_Empresa().obterEmpresa((int)id);
            if (tb_empresa == null)
            {
                return RedirectToAction("Index");
            }
           
            return View(tb_empresa);
        }

        // POST: Empresa/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "INFRA,DIRETOR-INFRA")]
        public ActionResult Edit([Bind(Include = "id,id_endereco,id_contato,razao_social,nome_fantasia,cnpj,inscricao_estadual,inscricao_municipal,atividade_principal,tb_contato,tb_endereco")] tb_empresa tb_empresa)
        {
            tb_empresa tb_empresatwo = new Manter_Empresa().obterEmpresa((int)tb_empresa.id);
            if (ModelState.IsValid)
            {
                new Manter_Contato().editar(tb_empresa.tb_contato, tb_empresatwo.id_contato);
                new Manter_Endereco().editar(tb_empresa.tb_endereco, tb_empresatwo.id_endereco);


                new Manter_Empresa().editar(tb_empresa, tb_empresatwo.id_endereco, tb_empresatwo.id_contato);
                return RedirectToAction("Index");
            }
            return View(tb_empresa);
        }

        // GET: Empresa/Delete/5
        [Authorize(Roles = "INFRA,DIRETOR-INFRA")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            tb_empresa tb_empresa = new Manter_Empresa().obterEmpresa((int)id);
            if (tb_empresa == null)
            {
                return RedirectToAction("Index");
            }
            return View(tb_empresa);
        }

        // POST: Empresa/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "INFRA,DIRETOR-INFRA")]
        public ActionResult DeleteConfirmed(int id)
        {
            new Manter_Empresa().remover(id);
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
