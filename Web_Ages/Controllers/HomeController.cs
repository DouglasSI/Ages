using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using Servico.Manter;
namespace Web_Ages.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            tb_usuario user = new Manter_Usuario().obterUsuario("FABRICIO", "1234");
            return View();
        }

    }
}
