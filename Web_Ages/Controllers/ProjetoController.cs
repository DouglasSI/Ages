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
using Servico;
using System.IO;

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
            //ViewBag.Projetos = new Manter_Projeto().obterProjetos();
            return View(new Manter_Projeto().obterProjetos());
        }
        // GET: Projeto/Details/5
        public ActionResult DetailsOrcamentosdoProjeto(int? id)
        {
            tb_projeto projeto = new Manter_Projeto().obterProjeto((int)id);
            projeto.tb_orcamento = projeto.tb_orcamento.Where(f => f.id_status.Equals(2)).ToList();
            return View(projeto);
        }
        [HttpPost]
        public ActionResult DetailsOrcamentosdoProjeto(tb_orcamento id_orcamento)
        {
            int param = int.Parse(this.Request.Params["orcamento"]);
            id_orcamento = new Manter_Orcamento().obterOrcamento(param);
            var ass = id_orcamento.GetType();

            return RedirectToAction("Aditivo", new { id_orcamento = id_orcamento.id });

        }

        [HttpGet]
        public ActionResult Aditivo(int? id_orcamento)
        {

            tb_orcamento orcamento = new Manter_Orcamento().obterOrcamento((int)id_orcamento);
            @ViewBag.orcamento = orcamento;
            @ViewBag.id_forma_pagamento = new SelectList(new Manter_FormaPagamento().obterFormasPag(), "id", "descricao");
            return View();
            // ir numa view para dar entrada na tabela material, o valor vai descontar
            // da tabela fatura,

        }
        [HttpPost]
        public ActionResult Aditivo(tb_fatura tb_fatura)
        {
            @ViewBag.id_forma_pagamento = new SelectList(new Manter_FormaPagamento().obterFormasPag(), "id", "descricao");
            if (ModelState.IsValid)
            {
                tb_fatura.tb_forma_pagamento = new Manter_FormaPagamento().obterFormaPagamento(tb_fatura.id_forma_pagamento);

                tb_fatura.is_aditivo = true;
                tb_fatura.data_cadastro = DateTime.Now;
                tb_fatura.tb_usuario = new Manter_Usuario().obterUsuario(User.Identity.Name);
                tb_fatura.id_usuario = tb_fatura.tb_usuario.id;

                new Manter_().PersistirFatura(tb_fatura);
                tb_orcamento r = new Manter_Orcamento().obterOrcamento(tb_fatura.id_orcamento);
                return RedirectToAction("DetailsOrcamentosdoProjeto", new { id = r.id_projeto });
            }
            return View();


        }
        [HttpGet]
        public ActionResult Autorizar(int? id_fatura)
        {
            tb_fatura fatura = new Manter_Fatura().ObterFatura((int)id_fatura);
            @ViewBag.tb_fatura = fatura;

            return View(fatura);


        }
        [HttpPost]

        public ActionResult Autorizar(tb_fatura tb_fatura)
        {
            tb_fatura = new Manter_Fatura().ObterFatura((int)tb_fatura.id);
            //tb_fatura.tb_usuario = new Manter_Usuario().obterUsuario(User.Identity.Name);
            //tb_fatura.id_usuario = tb_fatura.tb_usuario.id;
            tb_fatura.autorizado = true;
            new Manter_Fatura().autorizar(tb_fatura);
            tb_orcamento r = new Manter_Orcamento().obterOrcamento(tb_fatura.id_orcamento);
            return RedirectToAction("DetailsOrcamentosdoProjeto", new { id = r.id_projeto });
        }
        
        public ActionResult Material(int? id_fatura)
        {
            tb_fatura fatura = new Manter_Fatura().ObterFatura((int)id_fatura);
            @ViewBag.tb_fatura = fatura;


            if (Model.Super.SuperAnexo.anexos == null)
                Model.Super.SuperAnexo.clearAnexos();

            @ViewBag.tb_anexo = Model.Super.SuperAnexo.anexos;

            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Material(tb_compra compra)
        {
            tb_fatura fatura = new Manter_Fatura().ObterFatura(compra.id_fatura);

            fatura.valor_pendente -= compra.valor;
            compra.tb_fatura = fatura;

            new Manter_().PersistirCompra(compra);
            // update na tabela fatura, agora  financeiro pode realizar pagamento
            tb_orcamento r = new Manter_Orcamento().obterOrcamento(fatura.id_orcamento);
            return RedirectToAction("DetailsOrcamentosdoProjeto", new { id = r.id_projeto });
        }


        public ActionResult Baixa(int? id_fatura)
        {
            tb_fatura fatura = new Manter_Fatura().ObterFatura((int)id_fatura);

            //vai para outra view para dar entrada nos anexos e confirmar a baixa da fatura




            return View();
        }
        public ActionResult DetailsFaturasServicos(int? id)
        {
            ViewBag.Faturas = new Manter_Fatura().ObterFaturasporOrcamento((int)id);
            ViewBag.Servicos = new Manter_Orcamento_Servico().ObterServicosporOrcamento((int)id);
            return View(new Manter_Orcamento().obterOrcamento((int)id));
        }
        public ActionResult DetalheOrcamento(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            return RedirectToAction("Details", "Orcamento", new { id });

        }
        public ActionResult DetailsSomenteVisualizar(int? id)
        {
            tb_projeto tb_projeto = new Manter_Projeto().obterProjeto((int)id);

            return View(tb_projeto);
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return PartialView("Details");
            }
            tb_projeto tb_projeto = new Manter_Projeto().obterProjeto((int)id);
            //tb_projeto.tb_orcamento = new Manter_Orcamento().obterOrcamentos((int)id);

            if (tb_projeto == null)
            {
                return HttpNotFound();
            }
            return PartialView(tb_projeto);
        }
        [HttpPost]
        public ActionResult Details(tb_projeto projeto)
        {
            if (projeto == null)
            {
                return PartialView("Details");
            }
            tb_projeto tb_projeto = new Manter_Projeto().obterProjeto(projeto.id);
            //tb_projeto.tb_orcamento = new Manter_Orcamento().obterOrcamentos((int)id);

            if (tb_projeto == null)
            {
                return HttpNotFound();
            }
            if (Request.Form["Infra"] != null)
            {
                return RedirectToAction("Create", "Orcamento", new { id_projeto = projeto.id });
            }
            else if (Request.Form["Diretor"] != null)
            {
                new Manter_Projeto().editarStatus(tb_projeto, 3);
                return RedirectToAction("Listar", "Projeto");
            }

            return PartialView("Details");
        }

        
        public ActionResult CreateAnexo()
        {
            if (Model.Super.SuperAnexo.anexos == null)
                Model.Super.SuperAnexo.clearAnexos();
            @ViewBag.tb_anexo = Model.Super.SuperAnexo.anexos;
            return PartialView();
        }
        [HttpPost]
        public ActionResult CreateAnexo(tb_anexo tb_anexo, HttpPostedFileBase file)
        {


            if (file != null && file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                tb_anexo.titulo = DateTime.Now.ToShortDateString().Replace('/', '_') + "_" + tb_anexo.titulo;
                tb_anexo.tipo = file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
                var path = Path.Combine(Server.MapPath("/arquivos/"), tb_anexo.titulo + "." + tb_anexo.tipo);



                file.SaveAs(path);
            }

            if (ModelState.IsValid)
            {
                tb_anexo.data_cadastro = DateTime.Now;
                tb_anexo.tb_usuario = new Manter_Usuario().obterUsuario(User.Identity.Name);
                tb_anexo.id_usuario = tb_anexo.tb_usuario.id;

                if (Model.Super.SuperAnexo.anexos == null)
                    Model.Super.SuperAnexo.clearAnexos();
                Model.Super.SuperAnexo.anexos.Add(tb_anexo);

                return ListarAnexos();
            }
            return ListarAnexos();
        }
        public ActionResult ListarAnexos()
        {
            @ViewBag.tb_anexo = Model.Super.SuperAnexo.anexos;
            return PartialView("ListarAnexos");

        }

        // GET: Projeto/Create
        public ActionResult Create()
        {
            ViewBag.id_campi = new SelectList(new Manter_Campi().obterCampis(), "id", "nome_fantasia".ToUpper());
            ViewBag.id_status = new SelectList(new List<tb_status_projeto>() { new Manter_Status().obterById(1) }, "id", "descricao");
            ViewBag.id_usuario = new SelectList(new List<tb_usuario>() { new Manter_Usuario().obterUsuario(User.Identity.Name) }, "id", "nome");
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
        public ActionResult DeleteOrcamento(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            return RedirectToAction("Delete", "Orcamento", new { id });
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
