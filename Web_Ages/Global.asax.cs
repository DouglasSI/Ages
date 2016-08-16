using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Model;

namespace Web_Ages
{

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            Model.Super.SuperOrcamento.orcamento = new Model.tb_orcamento();
            Model.Super.SuperOrcamento.orcamento.tb_orcamento_servico = new List<Model.tb_orcamento_servico>();
            Model.Super.SuperOrcamento.orcamento.tb_fatura = new List<Model.tb_fatura>();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            

        }
    }
}