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
    public class FormaPagamentoController : Controller
    {
        // GET: FormaPagamento
        public ActionResult Index()
        {   
            return PartialView(new Manter_FormaPagamento().obterFormasPag());
        }

        // GET: FormaPagamento/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            tb_forma_pagamento tb_forma = new Manter_FormaPagamento().obterFormaPagamento((int)id);
            if (tb_forma == null)
            {
                return RedirectToAction("Index");
            }
            return View(tb_forma);
        }

        // GET: FormaPagamento/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FormaPagamento/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        
        public ActionResult Create([Bind(Include = "id,descricao,banco,agencia,conta_corrente,digito")] tb_forma_pagamento tb_forma_pagamento)
        {
            tb_forma_pagamento.ativo = true;
            if (ModelState.IsValid)
            {
                
                new Manter_FormaPagamento().cadastrar(tb_forma_pagamento);

                return RedirectToAction("Index");
            }
            return View(tb_forma_pagamento);
        }

        // GET: FormaPagamento/Edit/5
        public ActionResult Edit(int? id)
        {

            if (id == null)
            {
                return RedirectToAction("Index");
            }
            tb_forma_pagamento formaPagamento = new Manter_FormaPagamento().obterFormaPagamento((int)id);
            if (formaPagamento == null)
            {
                return RedirectToAction("Index");
            }
            return View(formaPagamento);
        }

        // POST: FormaPagamento/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        
        public ActionResult Edit([Bind(Include = "id,descricao,banco,agencia,conta_corrente,digito,ativo")] tb_forma_pagamento tb_forma_pagamento)
        {
            if (ModelState.IsValid)
            {
                
                new Manter_FormaPagamento().editar(tb_forma_pagamento);
                return RedirectToAction("Index");
            }

            return View(tb_forma_pagamento);
        }

        // GET: FormaPagamento/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            tb_forma_pagamento tb_forma_pagamento = new Manter_FormaPagamento().obterFormaPagamento((int)id);
            if (tb_forma_pagamento == null)
            {
                return RedirectToAction("Index");
            }
            return View(tb_forma_pagamento);
        }

        // POST: FormaPagamento/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            new Manter_FormaPagamento().remover(id);
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
