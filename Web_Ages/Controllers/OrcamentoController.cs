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
using System.IO;
using Servico;

namespace Web_Ages.Controllers
{
    public class OrcamentoController : Controller
    {

        // GET: Orcamento/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {

                return RedirectToAction("Projetos");
            }

            tb_orcamento orc = new Manter_Orcamento().obterOrcamento((int)id);
            orc.tb_projeto = new Manter_Projeto().obterProjeto(orc.id_projeto);
            orc.tb_projeto.tb_campi = new Manter_Campi().obterCampi(orc.tb_projeto.id_campi);
            orc.tb_usuario = new Manter_Usuario().obterUsuarioByid(orc.id_usuario);
            orc.tb_projeto.tb_usuario = new Manter_Usuario().obterUsuarioByid(orc.tb_projeto.id_usuario);
           // orc.tb_fatura = new Manter_Fatura().ObterFaturasporOrcamento((int)orc.id);


            
            return View(orc);


        }
        [HttpPost]
        public ActionResult Details(tb_orcamento orcamento)
        {
            tb_orcamento orc = new Manter_Orcamento().obterOrcamento((int)orcamento.id);
            if (Request.Form["aceitar"] != null)
            {

                new Manter_Orcamento().editarStatus(orc, 2);
                return RedirectToAction("Details", "Projeto", new { id = orc.id_projeto });
            }
            else
                if (Request.Form["rejeitar"] != null)
                {
                    new Manter_Orcamento().editarStatus(orc, 3);
                    return RedirectToAction("Details", "Projeto", new { id = orc.id_projeto });
                }
            return View(orcamento);
        }
        public ActionResult EnviarParaDetailsProjeto(int? id)
        {
            return RedirectToAction("Details", "Projeto", new { id = id });
        }

        // GET: Orcamento/Create
        [Authorize(Roles = "INFRA,DIRETOR-INFRA")]
        public ActionResult Create(int? id_projeto)
        {
            Model.Super.SuperOrcamento.orcamento = new tb_orcamento();
            
            @ViewBag.tb_fatura = new List<tb_fatura>();
            ViewBag.id_empresa = new SelectList(new Manter_Empresa().obterEmpresas(), "id", "nome_fantasia");
            ViewBag.id_servico = new SelectList(new Manter_Servico().obterServicos(), "id", "titulo");
            ViewBag.id_forma_pagamento = new SelectList(new Manter_FormaPagamento().obterFormasPag(), "id", "descricao");

            ViewBag.id_projeto = new Manter_Projeto().obterProjeto((int)id_projeto);
            ViewBag.id_campi = new Manter_Campi().obterCampis().Where(x => x.id == ((tb_projeto)ViewBag.id_projeto).id_campi).FirstOrDefault();
            ViewBag.id_status = new Manter_Status().obterByIdOrcamento(1);
            ViewBag.id_usuario = new Manter_Usuario().obterUsuario(User.Identity.Name);

            List<tb_orcamento_servico> hList = Model.Super.SuperOrcamento.orcamento.tb_orcamento_servico.ToList();
            @ViewBag.tb_orcamento_servico = hList;



            Models.TempAnexo.models = new List<Models.ViewModel>();

            @ViewBag.tb_anexo = Web_Ages.Models.TempAnexo.models;
            return PartialView();
        }
        [HttpPost]
        [Authorize(Roles = "INFRA,DIRETOR-INFRA")]
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
                tb_orcamento.valor = Convert.ToDecimal((Request.Form["valor"]).Replace(",", "").Replace(".", ","));
                ModelState["valor"].Errors.Clear();

                if (ModelState.IsValid)
                {
                    Servico.Manter_ m = new Servico.Manter_();
                    tb_orcamento.id_usuario = userr.id;
                    tb_orcamento.id_status = 1;
                    tb_orcamento.tb_orcamento_servico = Model.Super.SuperOrcamento.orcamento.tb_orcamento_servico;
                    tb_orcamento.tb_fatura = Model.Super.SuperOrcamento.orcamento.tb_fatura.ToList();

                    foreach (Models.ViewModel model in Models.TempAnexo.models)
                    {
                        var fileName = Path.GetFileName(model.file.FileName);
                        using (Stream inputStream = model.file.InputStream)
                        {
                            MemoryStream memoryStream = inputStream as MemoryStream;
                            if (memoryStream == null)
                            {
                                memoryStream = new MemoryStream();
                                inputStream.CopyTo(memoryStream);
                            }
                            string cont = "/" + Manter_.tipo.orcamento.ToString().ToUpper() + "/";
                            FileStream file = new FileStream(Path.Combine(Server.MapPath("~/App_Data/Anexos" + cont), fileName), FileMode.Create, FileAccess.Write);
                            model.tb_anexo.caminho = Path.Combine(Server.MapPath("~/App_Data/Anexos" + cont));
                            
                            memoryStream.WriteTo(file);
                            file.Close();
                            memoryStream.Close();
                        }
                        tb_orcamento.tb_anexo.Add(model.tb_anexo);
                    }
                    m.PersistirGrupo(tb_orcamento);
                        
                    
                    return RedirectToAction("Details", "Projeto", new { id = tb_orcamento.id_projeto });
                }
            }
            return View(tb_orcamento);
        }
        [Authorize(Roles = "INFRA,DIRETOR-INFRA")]
        public ActionResult CreateServico()
        {
            
            ViewBag.id_servico = new SelectList(new Manter_Servico().obterServicos(), "id", "titulo");
            return PartialView();
        }

        [Authorize(Roles = "INFRA,DIRETOR-INFRA")]
        public ActionResult CreateFatura()
        {
            @ViewBag.id_forma_pagamento = new SelectList(new Manter_FormaPagamento().obterFormasPag(), "id", "descricao");
            Model.Super.SuperOrcamento.orcamento.tb_fatura.Clear();
            List<tb_fatura> hList = Model.Super.SuperOrcamento.orcamento.tb_fatura.ToList();
            @ViewBag.tb_fatura = hList;
            return PartialView();

        }
        [HttpPost]
        [Authorize(Roles = "INFRA,DIRETOR-INFRA")]
        public ActionResult CreateFatura(tb_fatura tb_fatura)
        {
            @ViewBag.id_forma_pagamento = new SelectList(new Manter_FormaPagamento().obterFormasPag(), "id", "descricao");
            tb_fatura.valor_inicial = Convert.ToDecimal((Request.Form["valor_inicial"]).Replace(",", "").Replace(".", ","));
            ModelState["valor_inicial"].Errors.Clear();
            if (ModelState.IsValid)
            {   
                tb_fatura.tb_forma_pagamento = new Manter_FormaPagamento().obterFormaPagamento(tb_fatura.id_forma_pagamento);
                tb_fatura.is_aditivo = false;
                tb_fatura.data_cadastro = DateTime.Now;
                Model.Super.SuperOrcamento.orcamento.tb_fatura.Add(tb_fatura);
                return ListarFaturas();


            }
            List<tb_fatura> hList = Model.Super.SuperOrcamento.orcamento.tb_fatura.ToList();
            @ViewBag.tb_fatura = hList;
            return ListarFaturas();
        }
        public ActionResult ListarFaturas()
        {

            List<tb_fatura> hList = Model.Super.SuperOrcamento.orcamento.tb_fatura.ToList();

            @ViewBag.tb_fatura = hList;

            return PartialView("ListarFaturas");

        }
        public FileResult Download(int? id)
        {


            int _arquivoId = Convert.ToInt32((int)id);
            tb_anexo tb_anexo = new Manter_().getAnexo((int)id);

            string contentType = "application/" + tb_anexo.tipo;
            return File(tb_anexo.caminho + tb_anexo.nome_arquivo, contentType, tb_anexo.nome_arquivo);
        }
        public ActionResult Remover(int? id)
        {


            int _arquivoId = Convert.ToInt32((int)id);
            Web_Ages.Models.TempAnexo.models.RemoveAt(_arquivoId);
            @ViewBag.tb_anexo = Web_Ages.Models.TempAnexo.models;
            return ListarAnexos();

        }
        [Authorize(Roles = "INFRA,DIRETOR-INFRA")]
        public ActionResult RemoverFatura(int id)
        {


            
            List<tb_fatura> hList = Model.Super.SuperOrcamento.orcamento.tb_fatura.ToList();
            hList.RemoveAt(id);
            Model.Super.SuperOrcamento.orcamento.tb_fatura = hList;
            @ViewBag.tb_fatura = hList;


              @ViewBag.id_forma_pagamento = new SelectList(new Manter_FormaPagamento().obterFormasPag(), "id", "descricao");
           
             return ListarFaturas();
        }
        [HttpPost]
        [Authorize(Roles = "INFRA,DIRETOR-INFRA")]
        public ActionResult CreateServico(tb_orcamento_servico tb_orcamento_servico)
        {
            tb_orcamento_servico.valor_servico = Convert.ToDecimal( (Request.Form["valor_servico"]).Replace(",", "").Replace(".", ","));
            ModelState["valor_servico"].Errors.Clear();
            if (ModelState.IsValid)
            {
                tb_orcamento_servico.data_cadastro = DateTime.Now;
                tb_servico s = new Manter_Servico().obterServico(tb_orcamento_servico.id_servico ); //retorna um objeto servico
                List<tb_servico> svs = new Manter_Servico().obterServicos();
                ViewBag.id_servico = new SelectList(svs, "id", "titulo");
                tb_orcamento_servico.tb_servico = s;


                List<tb_orcamento_servico> hList = Model.Super.SuperOrcamento.orcamento.tb_orcamento_servico.ToList();
                for (int i = 0; i < hList.Count; i++)
                    if (hList[i].id_servico == tb_orcamento_servico.id_servico)
                        hList.RemoveAt(i);
                hList.Add(tb_orcamento_servico);
                Model.Super.SuperOrcamento.orcamento.tb_orcamento_servico = hList;
                @ViewBag.tb_orcamento_servico = hList;

            }
            return ListarServicos();
        }
        public ActionResult ListarServicos()
        {


            List<tb_orcamento_servico> hList = Model.Super.SuperOrcamento.orcamento.tb_orcamento_servico.ToList();
            @ViewBag.tb_orcamento_servico = hList;

            return PartialView("ListaServicosOrcamento");
        }

        public ActionResult RemoverServico(int id_servico)

        {
           
            List<tb_orcamento_servico> hList = Model.Super.SuperOrcamento.orcamento.tb_orcamento_servico.ToList();
            hList.RemoveAt(id_servico);
            Model.Super.SuperOrcamento.orcamento.tb_orcamento_servico = hList;
            @ViewBag.tb_orcamento_servico = hList;
          //  ((List<tb_orcamento_servico>)Model.Super.SuperOrcamento.orcamento.tb_orcamento_servico).RemoveAt(id_servico);
            return ListarServicos();

        }
        // POST: Orcamento/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        // GET: Orcamento/Edit/5
        [Authorize(Roles = "INFRA,DIRETOR-INFRA")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_orcamento tb_orcamento = new Manter_Orcamento().obterOrcamento((int)id);
            if (tb_orcamento == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_empresa = new SelectList(new List<tb_empresa>() { new Manter_Empresa().obterEmpresa(tb_orcamento.id_empresa) }, "id", "razao_social", tb_orcamento.id_empresa);
            ViewBag.id_projeto = new SelectList(new List<tb_projeto> { new Manter_Projeto().obterProjeto(tb_orcamento.id_projeto) });

            ViewBag.id_status = new SelectList(new List<tb_status_orcamento>() { new Manter_Status().obterByIdOrcamento(tb_orcamento.id_status) }, "id_status", "descricao");
            ViewBag.id_usuario = new SelectList(new List<tb_usuario>() { new Manter_Usuario().obterUsuario(User.Identity.Name) }, "id_usuario", "nome");
            return View(tb_orcamento);
        }

        // POST: Orcamento/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "INFRA,DIRETOR-INFRA")]
        public ActionResult Edit([Bind(Include = "id,id_usuario,id_status,id_empresa,id_projeto,valor,anotacao")] tb_orcamento tb_orcamento)
        {
            if (ModelState.IsValid)
            {
                new Manter_Orcamento().editar(tb_orcamento);

                return RedirectToAction("Index");
            }
            ViewBag.id_empresa = new SelectList(new List<tb_empresa>() { new Manter_Empresa().obterEmpresa(tb_orcamento.id_empresa) }, "id", "razao_social", tb_orcamento.id_empresa);
            ViewBag.id_projeto = new SelectList(new List<tb_projeto>  { new Manter_Projeto().obterProjeto(tb_orcamento.id_projeto)});

            ViewBag.id_status = new SelectList(new List<tb_status_orcamento>() { new Manter_Status().obterByIdOrcamento(tb_orcamento.id_status) }, "id_status", "descricao");
            ViewBag.id_usuario = new SelectList(new List<tb_usuario>() { new Manter_Usuario().obterUsuario(User.Identity.Name) }, "id_usuario", "nome");


            return View(tb_orcamento);
        }

        // GET: Orcamento/Delete/5
        [Authorize(Roles = "INFRA,DIRETOR-INFRA")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Listar");
            }
            tb_orcamento tb_orcamento = new Manter_Orcamento().obterOrcamento((int)id);

            if (tb_orcamento == null)
            {
                return RedirectToAction("Listar");
            }
            return View(tb_orcamento);

        }

        // POST: Orcamento/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "INFRA,DIRETOR-INFRA")]
        public ActionResult DeleteConfirmed(int id)
        {
            int id_projeto = 0;
            foreach (var item in new Manter_Fatura().ObterFaturasporOrcamento((int)id))
            {

                new Manter_Fatura().remover((int)item.id_orcamento);
            }
            foreach (var item in new Manter_Orcamento_Servico().ObterServicosporOrcamento((int)id))
            {
                new Manter_Orcamento_Servico().remover((int)item.id_orcamento);
            }
            tb_orcamento os = new Manter_Orcamento().obterOrcamento((int)id);
            id_projeto = os.id_projeto;

            new Manter_Orcamento().remover((int)id);
            return RedirectToAction("Details", "Projeto", new { id = id_projeto });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
              
            }
            base.Dispose(disposing);
        }

        public ActionResult CreateAnexo()
        {
            if (Models.TempAnexo.models == null)
                Models.TempAnexo.models = new List<Models.ViewModel>();
            @ViewBag.tb_anexo = Web_Ages.Models.TempAnexo.models;
            return PartialView();
        }
        [HttpPost]
        public ActionResult CreateAnexo(Models.ViewModel modelo)
        {
            if (Models.TempAnexo.models == null)
                Models.TempAnexo.models = new List<Models.ViewModel>();
            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase varr = Request.Files[0];
                Models.TempAnexo.model = new Models.ViewModel()
                {
                    file = varr,
                };
                string fileName = Path.GetFileName(Models.TempAnexo.model.file.FileName);
                string directory = Server.MapPath("~/App_Data/Anexos/"); //change ths to your actual upload folder
                Models.TempAnexo.model.tb_anexo = new tb_anexo()
                {
                    titulo = DateTime.Now.ToShortDateString().Replace('/', '_') + DateTime.Now.ToShortTimeString().Replace('/', '_') + "_" + fileName,
                    nome_arquivo = fileName,
                    caminho = directory,
                    tamanho = Models.TempAnexo.model.file.ContentLength,
                    tipo = Models.TempAnexo.model.file.FileName.Split('.')[Models.TempAnexo.model.file.FileName.Split('.').Length - 1],
                    tb_usuario = new Manter_Usuario().obterUsuario(User.Identity.Name),
                    data_cadastro = DateTime.Now,
                };
                Models.TempAnexo.model.tb_anexo.id_usuario = Models.TempAnexo.model.tb_anexo.tb_usuario.id;
                
                Models.TempAnexo.models.Add(Models.TempAnexo.model);
                
            }
            @ViewBag.tb_anexo = Web_Ages.Models.TempAnexo.models;
            return ListarAnexos();
        }
        [HttpGet]
        public ActionResult CreateAloneAnexo(int? id, Servico.Manter_.tipo tipo)
        {

            Models.TempAnexo.models = new List<Models.ViewModel>();
            Models.TempAnexo.model = new Models.ViewModel()
            {
                id = (int)id,
                tipo = tipo,
            };

            @ViewBag.tb_anexo = Web_Ages.Models.TempAnexo.models;

            return PartialView();
        }
        [HttpPost]
        public ActionResult CreateAloneAnexo(Models.ViewModel modelo)
        {
            if (modelo.file == null)
                return View();
            if (Models.TempAnexo.models == null)
                Models.TempAnexo.models = new List<Models.ViewModel>();
            if (Models.TempAnexo.model == null)
                Models.TempAnexo.model = new Models.ViewModel();

            Models.TempAnexo.model.file = modelo.file;

            string fileName = Path.GetFileName(Models.TempAnexo.model.file.FileName);

            string directory = Server.MapPath("~/App_Data/Anexos/"); //change ths to your actual upload folder
            Models.TempAnexo.model.tb_anexo = new tb_anexo()
            {
                titulo = DateTime.Now.ToShortDateString().Replace('/', '_') + DateTime.Now.ToShortTimeString().Replace('/', '_') + "_" + fileName,
                nome_arquivo = fileName,
                caminho = directory,
                tamanho = Models.TempAnexo.model.file.ContentLength,
                tipo = Models.TempAnexo.model.file.FileName.Split('.')[Models.TempAnexo.model.file.FileName.Split('.').Length - 1],
                tb_usuario = new Manter_Usuario().obterUsuario(User.Identity.Name),
                data_cadastro = DateTime.Now,
            };
            Models.TempAnexo.model.tb_anexo.id_usuario = Models.TempAnexo.model.tb_anexo.tb_usuario.id;

            using (Stream inputStream = Models.TempAnexo.model.file.InputStream)
            {
                MemoryStream memoryStream = inputStream as MemoryStream;
                if (memoryStream == null)
                {
                    memoryStream = new MemoryStream();
                    inputStream.CopyTo(memoryStream);
                }
                string cont = "/" + Models.TempAnexo.model.tipo.ToString().ToUpper() + "/";


                FileStream file = new FileStream(Path.Combine(Server.MapPath("~/App_Data/Anexos" + cont), fileName), FileMode.Create, FileAccess.Write);
                Models.TempAnexo.model.tb_anexo.caminho = Path.Combine(Server.MapPath("~/App_Data/Anexos" + cont));
                Directory.CreateDirectory(Path.GetDirectoryName(file.Name));
                memoryStream.WriteTo(file);
                file.Close();
                memoryStream.Close();
            }


            Models.TempAnexo.models.Add(new Models.ViewModel()
            {
                tb_anexo = new Manter_().PersistirAnexo(Models.TempAnexo.model.tb_anexo, Models.TempAnexo.model.tipo, Models.TempAnexo.model.id),
            }
                );
            Models.TempAnexo.model.file = null;

            @ViewBag.tb_anexo = Web_Ages.Models.TempAnexo.models;

            return View();
        }

        public ActionResult ListarAnexos()
        {
            if (Models.TempAnexo.models == null)
                Models.TempAnexo.models = new List<Models.ViewModel>();

            @ViewBag.tb_anexo = Web_Ages.Models.TempAnexo.models;
            return PartialView("ListarAnexos");

        }


    }
}
