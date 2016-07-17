using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using Servico.Manter;
using System.Web.Security;
namespace Web_Ages.Controllers
{
    
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
         
            return View();
        }
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Abandon(); // it will clear the session at the end of request
            return RedirectToAction("Index", "Home");
        }

    }
}
