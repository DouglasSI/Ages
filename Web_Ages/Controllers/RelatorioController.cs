using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Rotativa;
using Rotativa.Options;
using Model;
using Servico.Manter;

namespace Web_Ages.Controllers
{
    [Authorize(Roles = "Diretor")]
    public class RelatorioController : Controller
    {

        // GET: Relatorio
        [Authorize(Roles = "Diretor")]
        public ActionResult Index()
        {
            
            
            ViewBag.id_projeto = new SelectList(new Manter_Projeto().obterProjetos(), "id", "titulo");
            ViewBag.id_projetoFinalizado = new SelectList(new Manter_Projeto().obterFinalizados(), "id", "titulo");
            ViewBag.id_empresa = new SelectList(new Manter_Empresa().obterEmpresas(), "id", "nome_fantasia");
            
            return View();
        }

        [Authorize(Roles = "Diretor")]
        public ActionResult ListagemEmpresas(int? pagina, Boolean? gerarPDF)
        {
            var ListagemEmpresas = new Manter_Empresa().obterEmpresas();

            if (gerarPDF != true)
            {   
                //ser renderizado normalmente pelo browser
                //Definindo a paginação              
                int paginaQdteRegistros = 10;
                int paginaNumeroNavegacao = (pagina ?? 1);

                return View(ListagemEmpresas.ToPagedList(paginaNumeroNavegacao, paginaQdteRegistros));
            }
            else
            {
                int paginaNumero = 1;
                //gerarPDF
                var pdf = new ViewAsPdf
                {
                    ViewName = "ListagemEmpresas",
                    PageSize = Size.A4,
                    IsGrayScale = true,
                    Model = ListagemEmpresas.ToPagedList(paginaNumero, ListagemEmpresas.Count)
                };
                return pdf;
            }
        }

        [Authorize(Roles = "Diretor")]
        public ActionResult ProjetoValores(int? pagina, int? id)
        {
            var projeto = new Manter_Projeto().obterProjeto((int)id);
            var orcamentos = new Manter_Orcamento().obterOrcamentos(projeto.id);
            
                //ser renderizado normalmente pelo browser
                //Definindo a paginação              
                int paginaQdteRegistros = 10;
                int paginaNumeroNavegacao = (pagina ?? 1);

                return View(orcamentos.ToPagedList(paginaNumeroNavegacao, paginaQdteRegistros));
        }

        [Authorize(Roles = "Diretor")]
        public ActionResult ProjetoValoresFinalizados(int? pagina, int? id)
        {
            var projeto = new Manter_Projeto().obterProjeto((int)id);
            var orcamentos = new Manter_Orcamento().obterOrcamentos(projeto.id);
            int paginaQdteRegistros = 10;
            int paginaNumeroNavegacao = (pagina ?? 1);
            return View(orcamentos.ToPagedList(paginaNumeroNavegacao, paginaQdteRegistros));
        }

        [Authorize(Roles = "Diretor")]
        public ActionResult FornecedoresValores(int? pagina, int? id)
        {
            var orcamentos = new Manter_Orcamento().obterOrcamentosPorEmpresa((int) id);
            int paginaQdteRegistros = 10;
            int paginaNumeroNavegacao = (pagina ?? 1);
            return View(orcamentos.ToPagedList(paginaNumeroNavegacao, paginaQdteRegistros));
        }

    }
}