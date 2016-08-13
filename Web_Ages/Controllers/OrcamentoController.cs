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
using System.Web.Helpers;

namespace Web_Ages.Controllers
{
    public class OrcamentoController : Controller
    {
        private db_agesEntities2 db = new db_agesEntities2();

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
            
            tb_orcamento  orc = new Manter_Orcamento().obterOrcamento((int)id);
            orc.tb_projeto = new Manter_Projeto().obterProjeto(orc.id_projeto);
            orc.tb_projeto.tb_campi = new Manter_Campi().obterCampi(orc.tb_projeto.id_campi);
            orc.tb_usuario = new Manter_Usuario().obterUsuarioByid(orc.id_usuario);
            orc.tb_projeto.tb_usuario = new Manter_Usuario().obterUsuarioByid(orc.tb_projeto.id_usuario);
            //orc.tb_fatura = new Manter_Fatura().ObterFaturasporOrcamento((int)orc.id);
            //orc.tb_orcamento_servico = new Manter_Orcamento_Servico().ObterServicosporOrcamento((int)orc.id);

           return  View(orc);
            
           
        }
        [HttpPost]
        public ActionResult Details(tb_orcamento orcamento)
        {
            tb_orcamento orc = new Manter_Orcamento().obterOrcamento((int)orcamento.id);
            if (Request.Form["aceitar"] != null)
            {

                new Manter_Orcamento().editarStatus(orc, 2);
                return RedirectToAction("Details", "Projeto", new { id = orc. id_projeto });
            }
            else
                if (Request.Form["rejeitar"] != null)
            {
                new Manter_Orcamento().editarStatus(orc, 3);
                return RedirectToAction("Details", "Projeto", new { id = orc. id_projeto });
            }
            return View(orcamento);
        }
        public ActionResult EnviarParaDetailsProjeto(int? id)
        {
            return RedirectToAction("Details", "Projeto", new {id = id});
        }
      
        // GET: Orcamento/Create
        public ActionResult Create(int? id_projeto)
        {

            @ViewBag.tb_orcamento_servico = new List<tb_orcamento_servico>();
            @ViewBag.tb_fatura = new List<tb_fatura>();
            ViewBag.id_empresa = new SelectList(new Manter_Empresa().obterEmpresas(), "id", "nome_fantasia");
            ViewBag.id_servico = new SelectList(new Manter_Servico().obterServicos(), "id", "titulo");
            ViewBag.id_forma_pagamento = new SelectList(new Manter_FormaPagamento().obterFormasPag(), "id", "descricao");

            ViewBag.id_projeto = new Manter_Projeto().obterProjeto((int)id_projeto);
            ViewBag.id_campi = new Manter_Campi().obterCampis().Where(x => x.id == ((tb_projeto)ViewBag.id_projeto).id_campi).FirstOrDefault();
            ViewBag.id_status = new Manter_Status().obterByIdOrcamento(1);
            ViewBag.id_usuario = new Manter_Usuario().obterUsuario(User.Identity.Name);

            @ViewBag.tb_orcamento_servico = (List<tb_orcamento_servico>)Model.Super.SuperOrcamento.orcamento.tb_orcamento_servico;
            return PartialView();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Model.tb_orcamento tb_orcamento)
        {

            ViewBag.id_empresa = new SelectList(new Manter_Empresa().obterEmpresas(), "id", "nome_fantasia");
            ViewBag.id_servico = new SelectList(new Manter_Servico().obterServicos(), "id", "titulo");


            ViewBag.id_projeto = new Manter_Projeto().obterProjeto(tb_orcamento.id_projeto);
            ViewBag.id_campi = new Manter_Campi().obterCampis().Where(x => x.id == ((tb_projeto)ViewBag.id_projeto).id_campi).FirstOrDefault();
            ViewBag.id_status = new Manter_Status().obterByIdOrcamento(1);
            tb_usuario userr = new Manter_Usuario().obterUsuario(User.Identity.Name);
            ViewBag.id_usuario = userr;
            if (Request.Form["bt_submit_1"] != null)
            {
                if (ModelState.IsValid)
                {
                    Servico.Manter_ m = new Servico.Manter_();
                    tb_orcamento.id_usuario = userr.id;
                    tb_orcamento.id_status = 1;
                    tb_orcamento.tb_orcamento_servico = Model.Super.SuperOrcamento.orcamento.tb_orcamento_servico;
                    tb_orcamento.tb_fatura = Model.Super.SuperOrcamento.orcamento.tb_fatura.ToList();
                    m.PersistirGrupo(tb_orcamento);
                    return RedirectToAction("Details", "Projeto", new { id = tb_orcamento.id_projeto });
                }
            }
            return View(tb_orcamento);
        }

        public ActionResult CreateServico()
        {

            ViewBag.id_servico = new SelectList(new Manter_Servico().obterServicos(), "id", "titulo");
            return PartialView();
        }
        public ActionResult CreateFatura()
        {
            @ViewBag.id_forma_pagamento = new SelectList(new Manter_FormaPagamento().obterFormasPag(), "id", "descricao");

            return PartialView();
            
        }
        [HttpPost]
        public ActionResult CreateFatura(tb_fatura tb_fatura)
        {
            @ViewBag.id_forma_pagamento = new SelectList(new Manter_FormaPagamento().obterFormasPag(), "id", "descricao");
            if (ModelState.IsValid)
            {
                tb_fatura.tb_forma_pagamento = new Manter_FormaPagamento().obterFormaPagamento(tb_fatura.id_forma_pagamento);

                tb_fatura.is_aditivo = false;
                tb_fatura.data_cadastro = DateTime.Now;
                
                Model.Super.SuperOrcamento.orcamento.tb_fatura.Add(tb_fatura);
                @ViewBag.tb_fatura = Model.Super.SuperOrcamento.orcamento.tb_fatura;
                
                return ListarFaturas();
            }
            return ListarFaturas();

        }
        public ActionResult ListarFaturas()
        {
            @ViewBag.tb_fatura = Model.Super.SuperOrcamento.orcamento.tb_fatura;
            return PartialView("ListarFaturas");

        }
        
        public void RemoverFatura( int id)
        {
            foreach (tb_fatura ft in ((List<tb_fatura>)ViewBag.faturas))
                if (ft.id == id)
                    ((List<tb_fatura>)ViewBag.faturas).Remove(ft);
        }
        [HttpPost]
        public ActionResult CreateServico(tb_orcamento_servico tb_orcamento_servico)
        {

            if (ModelState.IsValid)
            {
                tb_orcamento_servico.data_cadastro = DateTime.Now;

                string id_servico = Request.Params.Get("id_servico");
                tb_servico s = new Manter_Servico().obterServico(int.Parse(id_servico)); //retorna um objeto servico
                List<tb_servico> svs = new Manter_Servico().obterServicos();

                ViewBag.id_servico = new SelectList(svs, "id", "titulo");

                tb_orcamento_servico.tb_servico = s;
                for (int i = 0; i < Model.Super.SuperOrcamento.orcamento.tb_orcamento_servico.Count; i++)
                    if (((List<tb_orcamento_servico>)Model.Super.SuperOrcamento.orcamento.tb_orcamento_servico)[i].
                        id_servico
                        == tb_orcamento_servico.id_servico)
                        ((List<tb_orcamento_servico>)Model.Super.SuperOrcamento.orcamento.tb_orcamento_servico).RemoveAt(i);

                Model.Super.SuperOrcamento.orcamento.tb_orcamento_servico.Add(tb_orcamento_servico);

                @ViewBag.tb_orcamento_servico = (List<tb_orcamento_servico>)Model.Super.SuperOrcamento.orcamento.tb_orcamento_servico;

                return ListarServicos();
            }
            return ListarServicos();

        }

        public ActionResult ListarServicos()
        {
            @ViewBag.tb_orcamento_servico = (List<tb_orcamento_servico>)Model.Super.SuperOrcamento.orcamento.tb_orcamento_servico;
            return PartialView("ListaServicosOrcamento");
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
            int id_projeto = 0;
            foreach (var item in new Manter_Fatura().ObterFaturasporOrcamento((int) id ))
            {
                
                new Manter_Fatura().remover((int)item.id_orcamento);
            }
            foreach (var item in new Manter_Orcamento_Servico().ObterServicosporOrcamento((int)id))
            {
                new Manter_Orcamento_Servico().remover((int)item.id_orcamento);
            }
            tb_orcamento os = new Manter_Orcamento().obterOrcamento((int)id);
            id_projeto = os.id_projeto;
           
            new Manter_Orcamento().remover((int) id);
            return RedirectToAction("Details", "Projeto", new { id = id_projeto });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult CreateAnexo()
        {
            Model.Super.SuperAnexo.clearAnexos();
            return PartialView();
        }
        [HttpPost]
        public ActionResult CreateAnexo(tb_anexo tb_anexo)
        {

            if (ModelState.IsValid)
            {
                tb_anexo.data_cadastro = DateTime.Now;
                tb_anexo.tb_usuario = new Manter_Usuario().obterUsuario(User.Identity.Name);
                tb_anexo.id_usuario = tb_anexo.tb_usuario.id;
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


    }
}
