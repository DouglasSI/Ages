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
using System.Threading.Tasks;
using System.Drawing;

namespace Web_Ages.Controllers
{
    [Authorize]
    public class ProjetoController : Controller
    {
        List<HttpPostedFileBase> arquivos = new List<HttpPostedFileBase>();
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
            return PartialView(new Manter_Projeto().obterProjetos());
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return PartialView("Details");
            }
            tb_projeto tb_projeto = new Manter_Projeto().obterProjeto((int)id);
            //tb_projeto.tb_orcamento = new Manter_Orcamento().obterOrcamentos((int)id);
            Models.TempAnexo.models = new List<Models.ViewModel>();
            @ViewBag.tb_anexo = Web_Ages.Models.TempAnexo.models;

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
        // GET: Projeto/Details/5
        public ActionResult DetailsOrcamentosdoProjeto(int? id)
        {
            tb_projeto projeto = new Manter_Projeto().obterProjeto((int)id);
            projeto.tb_orcamento = projeto.tb_orcamento.Where(f => f.id_status.Equals(2)).ToList();

            Models.TempAnexo.models = new List<Models.ViewModel>();
            @ViewBag.tb_anexo = Web_Ages.Models.TempAnexo.models;
            return PartialView(projeto);
            //return View(projeto); local de mudanca 1
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
        [Authorize(Roles = "INFRA,DIRETOR-INFRA")]
        public ActionResult Aditivo(int? id_orcamento)
        {

            tb_orcamento orcamento = new Manter_Orcamento().obterOrcamento((int)id_orcamento);
            @ViewBag.orcamento = orcamento;
            @ViewBag.id_forma_pagamento = new SelectList(new Manter_FormaPagamento().obterFormasPag(), "id", "descricao");


            Models.TempAnexo.models = new List<Models.ViewModel>();
            @ViewBag.tb_anexo = Web_Ages.Models.TempAnexo.models;

            return PartialView();

            // ir numa view para dar entrada na tabela material, o valor vai descontar
            // da tabela fatura,

        }
        [HttpPost]
        [Authorize(Roles = "INFRA,DIRETOR-INFRA")]
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
                        
                        FileStream file = new FileStream(Path.Combine(Server.MapPath("~/App_Data/Anexos/ADITIVO/"), fileName), FileMode.Create, FileAccess.Write);
                        model.tb_anexo.caminho = Path.Combine(Server.MapPath("~/App_Data/Anexos/ADITIVO"));
                        memoryStream.WriteTo(file);
                        file.Close();
                        memoryStream.Close();
                    }
                    new Manter_().PersistirAnexo(model.tb_anexo, Manter_.tipo.fatura, tb_fatura.id);
                }
                return RedirectToAction("DetailsOrcamentosdoProjeto", new { id = r.id_projeto });
            }
            return View();


        }
        [HttpGet]
        [Authorize(Roles = "DIRETOR-INFRA")]
        public ActionResult Autorizar(int? id_fatura)
        {
            tb_fatura fatura = new Manter_Fatura().ObterFatura((int)id_fatura);
            @ViewBag.tb_fatura = fatura;

            Models.TempAnexo.models = new List<Models.ViewModel>();
            @ViewBag.tb_anexo = Web_Ages.Models.TempAnexo.models;

            return PartialView();
            //return View(fatura);mudanca 02
        }
        [HttpPost]
        [Authorize(Roles = "DIRETOR-INFRA")]
        public ActionResult Autorizar(tb_fatura tb_fatura)
        {
            var param = int.Parse(this.Request.Params["id_fatura"]);
            tb_fatura = new Manter_Fatura().ObterFatura(param);

            tb_fatura.autorizado = true;
            new Manter_Fatura().autorizar(tb_fatura);
            tb_orcamento r = new Manter_Orcamento().obterOrcamento(tb_fatura.id_orcamento);

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
                    string cont = "/" + Manter_.tipo.fatura.ToString().ToUpper() + "/";
                    FileStream file = new FileStream(Path.Combine(Server.MapPath("~/App_Data/Anexos" + cont), fileName), FileMode.Create, FileAccess.Write);
                    model.tb_anexo.caminho = Path.Combine(Server.MapPath("~/App_Data/Anexos" + cont));
                    memoryStream.WriteTo(file);
                    file.Close();
                    memoryStream.Close();
                }
                new Manter_().PersistirAnexo(model.tb_anexo, Manter_.tipo.fatura, tb_fatura.id);
            }


            return RedirectToAction("DetailsOrcamentosdoProjeto", new { id = r.id_projeto });
        }
        
        [Authorize(Roles = "INFRA,DIRETOR-INFRA")]
        
        public ActionResult Material(int? id_fatura)
        {
            tb_fatura fatura = new Manter_Fatura().ObterFatura((int)id_fatura);
            @ViewBag.tb_fatura = fatura;
            Models.TempAnexo.models = new List<Models.ViewModel>();
            @ViewBag.tb_anexo = Web_Ages.Models.TempAnexo.models;
            return PartialView();
        }
        public async Task SaveFileContentsAsync(string filePath, Stream stream)
        {
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await stream.CopyToAsync(fileStream);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "INFRA,DIRETOR-INFRA")]
        public ActionResult Material(tb_compra compra)
        {
            if (Request.Form["bt_submit_1"] != null)
            {
                if (ModelState.IsValid)
                {
                    new Manter_().PersistirCompra(compra);
                    tb_fatura fatura = new Manter_Fatura().ObterFatura(compra.id_fatura);
                    compra.tb_fatura = fatura;
                    tb_orcamento r = new Manter_Orcamento().obterOrcamento(fatura.id_orcamento);
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
                            string cont = "/" + Manter_.tipo.compra.ToString().ToUpper() + "/";
                            FileStream file = new FileStream(Path.Combine(Server.MapPath("~/App_Data/Anexos" + cont), fileName), FileMode.Create, FileAccess.Write);
                            model.tb_anexo.caminho = Path.Combine(Server.MapPath("~/App_Data/Anexos" + cont));
                            memoryStream.WriteTo(file);
                            file.Close();
                            memoryStream.Close();
                        }
                        
                        new Manter_().PersistirAnexo(model.tb_anexo, Manter_.tipo.compra, compra.id);
                    }
                    return RedirectToAction("DetailsOrcamentosdoProjeto", "Projeto", new { id = r.id_projeto });
                }
            }
            return View(compra);
        }
        [Authorize(Roles = "FINANCEIRO")]
        public ActionResult Baixa(int? id_fatura)
        {
            tb_fatura fatura = new Manter_Fatura().ObterFatura((int)id_fatura);
            Models.TempAnexo.models = new List<Models.ViewModel>();
            @ViewBag.tb_anexo = Web_Ages.Models.TempAnexo.models;
            @ViewBag.tb_fatura = fatura;
            return PartialView();



            //return View();mudanca
        }
        [HttpPost]
        [Authorize(Roles = "FINANCEIRO")]
        public ActionResult Baixa(tb_fatura tb_fatura)
        {
            var param = int.Parse(this.Request.Params["id_fatura"]);
            tb_fatura = new Manter_Fatura().ObterFatura(param);

            @ViewBag.tb_anexo = Web_Ages.Models.TempAnexo.models;
            @ViewBag.tb_fatura = tb_fatura;
            if (Models.TempAnexo.models.Count == 0)
                return PartialView();
            

            tb_fatura.autorizado = true;
            tb_fatura.data_pagamento = DateTime.Now;
            tb_fatura.id_usuario = new Manter_Usuario().obterUsuario(User.Identity.Name).id;
            tb_fatura.anotacao += " [ BAIXA => " + tb_fatura.valor_pendente.ToString("0.00") + " ]";
            tb_fatura.valor_pendente = 0;
            tb_fatura.encerrado = true;
            new Manter_Fatura().autorizar(tb_fatura);
            tb_orcamento r = new Manter_Orcamento().obterOrcamento(tb_fatura.id_orcamento);

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
                    string cont = "/" + Manter_.tipo.fatura.ToString().ToUpper() + "/BAIXA/";
                    FileStream file = new FileStream(Path.Combine(Server.MapPath("~/App_Data/Anexos/" + cont), fileName), FileMode.Create, FileAccess.Write);
                    model.tb_anexo.caminho = Path.Combine(Server.MapPath("~/App_Data/Anexos" + cont));
                    memoryStream.WriteTo(file);
                    file.Close();
                    memoryStream.Close();
                }
                new Manter_().PersistirAnexo(model.tb_anexo, Manter_.tipo.fatura, tb_fatura.id);
            }


            return RedirectToAction("DetailsOrcamentosdoProjeto", new { id = r.id_projeto });
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
            Models.TempAnexo.models = new List<Models.ViewModel>();
            @ViewBag.tb_anexo = Web_Ages.Models.TempAnexo.models;

            return PartialView(tb_projeto);
            //return View(tb_projeto); outra mudanca
        }
        [HttpPost]
        public ActionResult DetailsSomenteVisualizar(tb_projeto id_projeto)
        {
            int param = int.Parse(this.Request.Params["proposta"]);
            id_projeto = new Manter_Projeto().obterProjeto(param);
            var ass = id_projeto.GetType();
            Models.TempAnexo.models = new List<Models.ViewModel>();
            @ViewBag.tb_anexo = Web_Ages.Models.TempAnexo.models;

            return RedirectToAction("CreateAloneAnexo", new { id = param, tipo = Manter_.tipo.projeto });

        }
        public void Remover(int? id)
        {


            int _arquivoId = Convert.ToInt32((int)id);
            Web_Ages.Models.TempAnexo.models.RemoveAt(_arquivoId);
            @ViewBag.tb_anexo = Web_Ages.Models.TempAnexo.models;

        }
        public FileResult Download(int? id)
        {

        
            int _arquivoId = Convert.ToInt32((int)id);
            tb_anexo tb_anexo = new Manter_().getAnexo((int)id);

            string contentType = "application/"+tb_anexo.tipo;
            return File(tb_anexo.caminho + tb_anexo.nome_arquivo, contentType, tb_anexo.nome_arquivo);
        }
   
        [HttpGet]
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


                //string cont = "/" + Models.TempAnexo.model.tipo.ToString().ToUpper() + "/";


                //FileStream file = new FileStream(Path.Combine(Server.MapPath("~/App_Data/Anexos" + cont), fileName), FileMode.Create, FileAccess.Write);
                //Models.TempAnexo.model.tb_anexo.caminho = Path.Combine(Server.MapPath("~/App_Data/Anexos" + cont));


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

        public ActionResult Voltar()
        {

            return RedirectToAction("Listar");
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
                string cont = "/" +  Models.TempAnexo.model.tipo.ToString().ToUpper() + "/";


                FileStream file = new FileStream(Path.Combine(Server.MapPath("~/App_Data/Anexos" + cont), fileName), FileMode.Create, FileAccess.Write);
                Models.TempAnexo.model.tb_anexo.caminho = Path.Combine(Server.MapPath("~/App_Data/Anexos" + cont));
                Directory.CreateDirectory(Path.GetDirectoryName(file.Name));
                memoryStream.WriteTo(file);
                file.Close();
                memoryStream.Close();
            }
            

            Models.TempAnexo.models.Add(new Models.ViewModel() {
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

        public FileResult Image(string tipo)
        {
            switch (tipo)
            {
                default:
                    return File(Server.MapPath("~/Content/pic/" + tipo), "image/png");
            
            }
        }
        // GET: Projeto/Create
        [Authorize(Roles = "Diretor")]
        public ActionResult Create()
        {
            ViewBag.id_campi = new SelectList(new Manter_Campi().obterCampis(), "id", "nome_fantasia".ToUpper());
            ViewBag.id_status = new SelectList(new List<tb_status_projeto>() { new Manter_Status().obterById(1) }, "id", "descricao");
            ViewBag.id_usuario = new SelectList(new List<tb_usuario>() { new Manter_Usuario().obterUsuario(User.Identity.Name) }, "id", "nome");

            Models.TempAnexo.models = new List<Models.ViewModel>();
            @ViewBag.tb_anexo = Web_Ages.Models.TempAnexo.models;

            return PartialView();

            //return View();
        }
        // POST: Projeto/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Diretor")]
        public ActionResult Create([Bind(Include = "id,id_usuario,id_status,id_campi,titulo,anotacao,valor_estimado")] tb_projeto tb_projeto)
        {
            if (ModelState.IsValid)
            {
                new Manter_Projeto().cadastrar(tb_projeto);
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
                        string cont = "/" + Manter_.tipo.projeto.ToString().ToUpper() + "/";
                        FileStream file = new FileStream(Path.Combine(Server.MapPath("~/App_Data/Anexos" + cont), fileName), FileMode.Create, FileAccess.Write);
                        memoryStream.WriteTo(file);
                        file.Close();
                        memoryStream.Close();
                    }
                    new Manter_().PersistirAnexo(model.tb_anexo, Manter_.tipo.projeto, tb_projeto.id);
                }
                return RedirectToAction("Listar");
            }
            ViewBag.id_campi = new SelectList(new Manter_Campi().obterCampis(), "id", "nome_fantasia".ToUpper());
            ViewBag.id_status = new SelectList(new List<tb_status_projeto>() { new Manter_Status().obterById(1) }, "id_status", "descricao");
            ViewBag.id_usuario = new SelectList(new List<tb_usuario>() { new Manter_Usuario().obterUsuario(User.Identity.Name) }, "id_usuario", "nome");



            return View(tb_projeto);
        }
        // GET: Projeto/Edit/5
        [Authorize(Roles = "Diretor")]
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

            Models.TempAnexo.models = new List<Models.ViewModel>();
            @ViewBag.tb_anexo = Web_Ages.Models.TempAnexo.models;
            @ViewBag.projeto = tb_projeto;
            //return PartialView(tb_projeto);

            return View(tb_projeto);
        }
        // POST: Projeto/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Diretor")]
        public ActionResult Edit([Bind(Include = "id,id_usuario,id_status,id_campi,titulo,anotacao,valor_estimado")] tb_projeto tb_projeto)
        {
            if (ModelState.IsValid)
            {
                tb_projeto.id_usuario = new Manter_Usuario().obterUsuario(User.Identity.Name).id;
                new Manter_Projeto().editar(tb_projeto);
                if (Request.Form["sem_anexo"] != null)
                {
                    return RedirectToAction("Listar");


                }
                if (Request.Form["com_anexo"] != null)
                {
                    Models.TempAnexo.models = new List<Models.ViewModel>();
                    @ViewBag.tb_anexo = Web_Ages.Models.TempAnexo.models;
                    return RedirectToAction("CreateAloneAnexo", new { id = tb_projeto.id, tipo = Manter_.tipo.projeto });


                }

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
        [Authorize(Roles = "Diretor")]
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
        [Authorize(Roles = "Diretor")]
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
