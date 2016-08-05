using Model;
using Servico.Manter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web_Ages.Controllers
{
    public class AnexoController : Controller
    {

      
        // GET: Anexo
        public ActionResult Index()
        {
            return View();
        }

        // GET: Anexo/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Anexo/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Anexo/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Anexo/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Anexo/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Anexo/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Anexo/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
