using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using System.Web.Security;

namespace Web_Ages.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/

        /// <param name="returnURL"></param>
        /// <returns></returns>
        public ActionResult Login(string returnURL)
        {
            /*Recebe a url que o usuário tentou acessar*/
            ViewBag.ReturnUrl = returnURL;
            return View(new tb_usuario());
        }
        /// <param name="login"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(tb_usuario login, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var vLogin = new Servico.Manter.Manter_Usuario().obterUsuario(login.email, login.senha);
                /*Verificar se a variavel vLogin está vazia. Isso pode ocorrer caso o usuário não existe.
                Caso não exista ele vai cair na condição else.*/
                if (vLogin != null)
                {

                    FormsAuthentication.SetAuthCookie(vLogin.email, false);
                    if (Url.IsLocalUrl(returnUrl)
                    && returnUrl.Length > 1
                    && returnUrl.StartsWith("/")
                    && !returnUrl.StartsWith("//")
                    && returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    /*código abaixo cria uma session para armazenar o nome do usuário*/
                    Session["Nome"] = vLogin.nome;
                    /*código abaixo cria uma session para armazenar o sobrenome do usuário*/
                    Session["Sobrenome"] = vLogin.sobrenome;
                    /*retorna para a tela inicial do Home*/
                    return RedirectToAction("Index", "Home");

                }
                /*Else responsável por verificar se o usuário existe*/
                else
                {
                    /*Escreve na tela a mensagem de erro informada*/
                    ModelState.AddModelError("", "E-mail informado inválido!!!");
                    /*Retorna a tela de login*/
                    return View(new tb_usuario());
                }
            }
            /*Caso os campos não esteja de acordo com a solicitação retorna a tela de login com as mensagem dos campos*/
            return View(login);
        }


    }
}
