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
            tb_fatura.valor_inicial = Convert.ToDecimal((Request.Form["valor_inicial"]).Replace(",", "").Replace(".", ","));
            tb_fatura.valor_pendente = Convert.ToDecimal((Request.Form["valor_inicial"]).Replace(",", "").Replace(".", ","));
            ModelState["valor_inicial"].Errors.Clear();
            if (ModelState.IsValid)
            {
               
                tb_fatura.tb_forma_pagamento = new Manter_FormaPagamento().obterFormaPagamento(tb_fatura.id_forma_pagamento);

                tb_fatura.is_aditivo = true;
                tb_fatura.data_cadastro = DateTime.Now;
                tb_fatura.tb_usuario = new Manter_Usuario().obterUsuario(User.Identity.Name);
                tb_fatura.id_usuario = tb_fatura.tb_usuario.id;

                

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
                        model.tb_anexo.caminho = Path.Combine(Server.MapPath("~/App_Data/Anexos/ADITIVO/"));
                        memoryStream.WriteTo(file);
                        file.Close();
                        memoryStream.Close();
                    }
                    tb_fatura.tb_anexo.Add(model.tb_anexo);
                }
                new Manter_().PersistirFatura(tb_fatura);
                tb_orcamento r = new Manter_Orcamento().obterOrcamento(tb_fatura.id_orcamento);

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
       
        [HttpGet]
        [Authorize(Roles = "DIRETOR-INFRA")]
        public ActionResult AutorizarMaterial(int? id_material)
        {
            tb_compra  compra = new Manter_Compra().obterCompraInt((int)id_material);
            @ViewBag.tb_compra = compra;
            ViewBag.id_empresa = new SelectList(new Manter_Empresa().obterEmpresas(), "id", "nome_fantasia");
            Models.TempAnexo.models = new List<Models.ViewModel>();
            @ViewBag.tb_anexo = Web_Ages.Models.TempAnexo.models;

            return PartialView();
            //return View(fatura);mudanca 02
        }
        [HttpPost]
        [Authorize(Roles = "DIRETOR-INFRA")]
        public ActionResult AutorizarMaterial(tb_compra tb_compra)
        {
                var param = int.Parse(this.Request.Params["id_material"]);
                tb_compra = new Manter_Compra().obterCompraInt(param);
           
                tb_compra.autorizado = true;
                tb_compra.data_autorizacao = DateTime.Now;
                new Manter_Compra().editar(tb_compra);
                tb_fatura fat = new Manter_Fatura().ObterFatura((int)tb_compra.id_fatura);
                tb_orcamento r = new Manter_Orcamento().obterOrcamento((int)fat.id_orcamento);
                ViewBag.id_empresa = new SelectList(new Manter_Empresa().obterEmpresas(), "id", "nome_fantasia");
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
                    new Manter_().PersistirAnexo(model.tb_anexo, Manter_.tipo.compra, tb_compra.id);
                }
                return RedirectToAction("DetailsOrcamentosdoProjeto", new { id = r.id_projeto });
       
           
        }

        [Authorize(Roles = "INFRA,DIRETOR-INFRA")]
        public ActionResult  DeleteMaterial(int? id_material)
        {
            tb_compra compra = new Manter_Compra().obterCompraInt((int)id_material);
            tb_fatura fat = new Manter_Fatura().ObterFatura((int)compra.id_fatura);
            fat.valor_pendente = compra.valor + fat.valor_pendente;
            new Manter_Fatura().Editar(fat);
            tb_orcamento orc = new Manter_Orcamento().obterOrcamento((int)fat.id_orcamento);
            
           
            new Manter_Compra().remover(id_material);
            return RedirectToAction("DetailsOrcamentosdoProjeto", new { id = orc.id_projeto });
        }

        [Authorize(Roles = "INFRA,DIRETOR-INFRA")]

        public ActionResult Material(int? id_fatura)
        {
            tb_fatura fatura = new Manter_Fatura().ObterFatura((int)id_fatura);
            @ViewBag.tb_fatura = fatura;
            Models.TempAnexo.models = new List<Models.ViewModel>();
            @ViewBag.tb_anexo = Web_Ages.Models.TempAnexo.models;
            ViewBag.id_empresa = new SelectList(new Manter_Empresa().obterEmpresas(), "id", "nome_fantasia");
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
                compra.valor = Convert.ToDecimal((Request.Form["valor"]).Replace(",", "").Replace(".", ","));
                ModelState["valor"].Errors.Clear();
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
        public ActionResult BaixaMaterial(int? id_material)
        {
            tb_compra compra = new Manter_Compra().obterCompraInt((int)id_material);
            Models.TempAnexo.models = new List<Models.ViewModel>();
            @ViewBag.tb_anexo = Web_Ages.Models.TempAnexo.models;
            @ViewBag.tb_compra = compra;
            ViewBag.id_empresa = new SelectList(new Manter_Empresa().obterEmpresas(), "id", "nome_fantasia");
            return PartialView();



            
        }

        [HttpPost]
        [Authorize(Roles = "FINANCEIRO")]
        public ActionResult BaixaMaterial(tb_compra tb_compra)
        {
            var param = int.Parse(this.Request.Params["id_material"]);
            tb_compra = new Manter_Compra().obterCompraInt(param);

            @ViewBag.tb_anexo = Web_Ages.Models.TempAnexo.models;
            @ViewBag.tb_compra = tb_compra;
            ViewBag.id_empresa = new SelectList(new Manter_Empresa().obterEmpresas(), "id", "nome_fantasia");
            if (Models.TempAnexo.models.Count == 0)
                return PartialView();


            tb_compra.autorizado = true;
            tb_compra.data_pagamento = DateTime.Now;
            
            tb_compra.anotacao += " [ BAIXA => " + tb_compra.valor.ToString("0.00") + " ]";
            
            tb_compra.encerrado = true;
            new Manter_Compra().editar(tb_compra);
            tb_fatura fat = new Manter_Fatura().ObterFatura((int)tb_compra.id_fatura);
            tb_orcamento r = new Manter_Orcamento().obterOrcamento(fat.id_orcamento);

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
                    string cont = "/" + Manter_.tipo.compra.ToString().ToUpper() + "/BAIXA/";
                    FileStream file = new FileStream(Path.Combine(Server.MapPath("~/App_Data/Anexos/" + cont), fileName), FileMode.Create, FileAccess.Write);
                    model.tb_anexo.caminho = Path.Combine(Server.MapPath("~/App_Data/Anexos" + cont));
                    memoryStream.WriteTo(file);
                    file.Close();
                    memoryStream.Close();
                }
                new Manter_().PersistirAnexo(model.tb_anexo, Manter_.tipo.compra, tb_compra.id);
            }


            return RedirectToAction("DetailsOrcamentosdoProjeto", new { id = r.id_projeto });
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
        public ActionResult Remover(int? id)
        {

            
            int _arquivoId = Convert.ToInt32((int)id);
            Web_Ages.Models.TempAnexo.models.RemoveAt(_arquivoId);
            @ViewBag.tb_anexo = Web_Ages.Models.TempAnexo.models;
            return ListarAnexos();

        }
        public ActionResult RemoverAnexoProjeto(int? id, int id_projeto )
        {
            new Manter_().removerAnexoProjeto((int)id);
            tb_projeto tb_projeto = new Manter_Projeto().obterProjeto(id_projeto);
            return RedirectToAction("Edit", new { id = id_projeto });
            


        }
        public FileResult Download(int? id)
        {


            int _arquivoId = Convert.ToInt32((int)id);
            tb_anexo tb_anexo = new Manter_().getAnexo((int)id);

            string contentType = "application/" + tb_anexo.tipo;
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

        public FileResult Image(string tipo)
        {
            switch (tipo)
            {
                default:
                    return File(Server.MapPath("~/Content/pic/" + tipo), "image/png");

            }
        }
        // GET: Projeto/Create
        [HttpGet]
        [Authorize(Roles = "Diretor")]
        public ActionResult Create()
        {
            ViewBag.id_campi = new SelectList(new Manter_Campi().obterCampis(), "id", "nome_fantasia".ToUpper());
            ViewBag.id_status = new Manter_Status().obterById(1);
            ViewBag.id_usuario = new Manter_Usuario().obterUsuario(User.Identity.Name);
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
        public ActionResult Create( tb_projeto tb_projeto)
        {
            #region if
            tb_projeto.valor_estimado = Convert.ToDecimal((Request.Form["valor_estimado"]).Replace(",", "").Replace(".", ","));
            ModelState["valor_estimado"].Errors.Clear();
            if (Request.Form["projeto"] != null && ModelState.IsValid)
            {

                tb_projeto.id_status = 1;
                tb_projeto.id_usuario = new Manter_Usuario().obterUsuario(User.Identity.Name).id;
                
                
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
                        model.tb_anexo.caminho = Path.Combine(Server.MapPath("~/App_Data/Anexos" + cont));
                        memoryStream.WriteTo(file);
                        file.Close();
                        memoryStream.Close();
                    }
                    new Manter_().PersistirAnexo(model.tb_anexo, Manter_.tipo.projeto, tb_projeto.id);
                }
                return RedirectToAction("Listar");
            }
            #endregion
            ViewBag.id_campi = new SelectList(new Manter_Campi().obterCampis(), "id", "nome_fantasia".ToUpper());
            ViewBag.id_status = new Manter_Status().obterById(1);
            ViewBag.id_usuario = new Manter_Usuario().obterUsuario(User.Identity.Name);

            
            


            return View(tb_projeto);
        }
        [HttpGet]
        [Authorize(Roles = "INFRA,DIRETOR-INFRA")]
        public ActionResult EditFatura(int? id_fatura)
        {
            @ViewBag.id_forma_pagamento = new SelectList(new Manter_FormaPagamento().obterFormasPag(), "id", "descricao");
            tb_fatura fat = new Manter_Fatura().ObterFatura((int)id_fatura);
            return View(fat);
        }
        [HttpPost]
        [Authorize(Roles = "INFRA,DIRETOR-INFRA")]
        public ActionResult EditFatura(tb_fatura fat)
        {
            fat.valor_inicial = Convert.ToDecimal((Request.Form["valor_inicial"]).Replace(",", "").Replace(".", ","));
            ModelState["valor_inicial"].Errors.Clear();
            
            tb_fatura faturaAnt = new Manter_Fatura().ObterFatura(fat.id);
            if (fat.data_prevista == DateTime.MinValue)
            {
                fat.data_prevista = faturaAnt.data_prevista;
            }
             
            string valor = fat.valor_inicial.ToString();
            string valor_novo=   valor.Substring(0, valor.Length - 2);
             fat.valor_inicial = Convert.ToDecimal(valor_novo);
            if (fat.valor_inicial != faturaAnt.valor_inicial) 
            { 
                if (fat.valor_inicial < faturaAnt.valor_inicial )
                {
                    fat.valor_pendente =faturaAnt.valor_pendente - (faturaAnt.valor_inicial-fat.valor_inicial);
                }
                else {
                    fat.valor_pendente = faturaAnt.valor_pendente + (fat.valor_inicial-faturaAnt.valor_inicial);
                }
             }
            if (fat.valor_inicial == faturaAnt.valor_inicial)
            {
                fat.valor_pendente = fat.valor_inicial;
            }
                new Manter_Fatura().Editar(fat);
                tb_orcamento orcamento = new Manter_Orcamento().obterOrcamento(fat.id_orcamento);
                return RedirectToAction("DetailsOrcamentosdoProjeto", new { id = orcamento.id_projeto });
         
        }
        [HttpGet]
        [Authorize(Roles = "INFRA,DIRETOR-INFRA")]
        public ActionResult EditServico(int? id_servico, int? id_orcamento)
        {
            tb_orcamento_servico orc_serv = new Manter_Orcamento_Servico().ObterOrcamentoServico(id_servico,id_orcamento);

            return View(orc_serv);
        }
        [HttpPost]
        [Authorize(Roles = "INFRA,DIRETOR-INFRA")]
        public ActionResult EditServico(tb_orcamento_servico orc_serv)
        {
            orc_serv.valor_servico = Convert.ToDecimal((Request.Form["valor_servico"]).Replace(",", "").Replace(".", ","));
            ModelState["valor_servico"].Errors.Clear();
            string valor = orc_serv.valor_servico.ToString();
            string valor_novo = valor.Substring(0, valor.Length - 2);
            orc_serv.valor_servico = Convert.ToDecimal(valor_novo);
            if (ModelState.IsValid)
            {

                new Manter_Orcamento_Servico().Editar(orc_serv);

            }
            tb_orcamento orcamento = new Manter_Orcamento().obterOrcamento(orc_serv.id_orcamento);


            return RedirectToAction("DetailsOrcamentosdoProjeto", new { id = orcamento.id_projeto });
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
            tb_projeto.valor_estimado = Convert.ToDecimal((Request.Form["valor_estimado"]).Replace(",", "").Replace(".", ","));
            ModelState["valor_estimado"].Errors.Clear();
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
