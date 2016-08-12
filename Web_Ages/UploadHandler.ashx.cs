using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using Web_Ages.Models;

namespace Web_Ages
{
    /// <summary>
    /// Summary description for UploadHandler
    /// </summary>
    public class UploadHandler : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            HttpPostedFile arquivo =
                context.Request.Files["Filedata"];
            string caminhoArquivo = PreVisualizacaoHelper
                .GetPathArquivoTemporario(context.Server,
                    arquivo.FileName);
            arquivo.SaveAs(caminhoArquivo);
            PreVisualizacaoHelper
                .AdicionarArquivoListaPreVisualizacao(
                     context.Session, caminhoArquivo);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}