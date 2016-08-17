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
    [Authorize(Roles = "TI")]
    public class UsuarioController : Controller
    {
 

        // GET: Usuario
        [Authorize(Roles = "TI")]
        public ActionResult Index()
        {

            return PartialView(new Manter_Usuario().obterUsuarios());
        }

        // GET: Usuario/Details/5
        [Authorize(Roles = "TI")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_usuario tb_usuario = new Manter_Usuario().obterUsuarioByid((int)id);
            if (tb_usuario == null)
            {
                return HttpNotFound();
            }
            return View(tb_usuario);
        }

        // GET: Usuario/Create
        [Authorize(Roles = "TI")]
        public ActionResult Create()
        {
            ViewBag.id_funcao = new SelectList(new Manter_Funcao().obterFuncoes(), "id", "nome");
            return View();
        }

        // POST: Usuario/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "TI")]
        public ActionResult Create([Bind(Include = "id,nome,senha,id_funcao,email,perfil,sobrenome")] tb_usuario tb_usuario)
        {
            if (ModelState.IsValid)
            {
                tb_usuario.ativo = true;
                new Servico.Manter.Manter_Usuario().cadastrar(tb_usuario);
                return RedirectToAction("Index");
            }

            ViewBag.id_funcao = new SelectList(new Manter_Funcao().obterFuncoes(), "id", "nome", tb_usuario.id_funcao);
            return View(tb_usuario);
        }

        // GET: Usuario/Edit/5
        [Authorize(Roles = "TI")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }


            tb_usuario tb_usuario = new Manter_Usuario().obterUsuarioByid((int)id);
            if (tb_usuario == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.id_funcao  = (new SelectList( new Manter_Funcao().obterFuncoes() , "id", "nome"));
            return View(tb_usuario);
        }

        // POST: Usuario/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "TI")]
        public ActionResult Edit([Bind(Include = "id,nome,senha,id_funcao,email,ativo,perfil,sobrenome")] tb_usuario tb_usuario)
        {
            if (ModelState.IsValid)
            {
                new Manter_Usuario().editar(tb_usuario);
                return RedirectToAction("Index");
            }
            ViewBag.id_funcao = (new SelectList(new Manter_Funcao().obterFuncoes(), "id", "nome"));
            return View(tb_usuario);
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
