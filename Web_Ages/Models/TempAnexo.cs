using Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.SessionState;

namespace Web_Ages.Models
{
    public static class TempAnexo
    {
        public static List<ViewModel> models { get; set; }

        public static ViewModel model { get; set; }

        
    }

    public static class ComplexAnexo{
    public static Servico.Manter_.tipo tipo {get;set;}

   public static     Object tb_ {get;set;}
    
    }

    public static class RepositorioArquivosHelper
    {

        public static void ConfirmarUploadArquivos(
           HttpServerUtility server,
           HttpSessionState session)
        {
            if (session["tb_anexosPreVisualizacao"] != null)
            {
                List<tb_anexo> arquivos =
                    (List<tb_anexo>)session["tb_anexosPreVisualizacao"];
                foreach (tb_anexo arqImagem in arquivos)
                {
                    File.Move(
                        arqImagem.caminho,
                        server.MapPath("~/Repositorio/") +
                        new FileInfo(arqImagem.caminho).Name);
                }

                session["tb_anexosPreVisualizacao"] = null;
            }
        }
        public static List<tb_anexo> GetArquivosRepositorio(
            HttpServerUtility server)
        {
            List<tb_anexo> arquivos = new List<tb_anexo>();

            DirectoryInfo diretorio = new DirectoryInfo(
                server.MapPath("~/Repositorio/"));
            arquivos.AddRange(
                from a in diretorio.GetFiles()
                orderby a.CreationTime descending
                select new tb_anexo()
                {
                    nome_arquivo = a.Name.Substring(
                        a.Name.IndexOf("]") + 1),
                    caminho = a.FullName,
                    url = "~/Repositorio/" + a.Name,
                    data_cadastro = a.CreationTime,
                    tamanho = (int) a.Length
                });

            return arquivos;
        }
    }

    public static class PreVisualizacaoHelper
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static string GetPathArquivoTemporario(
            HttpServerUtility server,
            string nomeOriginal)
        {
            return server.MapPath("~/PreVisualizacao/") +
                String.Format("[{0}]{1}",
                    DateTime.Now.ToString("yyyyMMddhhmmssfff"),
                    nomeOriginal);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void AdicionarArquivoListaPreVisualizacao(
            HttpSessionState session,
            string caminhotb_anexo)
        {
            List<tb_anexo> arquivos;
            if (session["tb_anexosPreVisualizacao"] != null)
            {
                arquivos = (List<tb_anexo>)
                    session["tb_anexosPreVisualizacao"];
            }
            else
            {
                arquivos = new List<tb_anexo>();
                session["tb_anexosPreVisualizacao"] = arquivos;
            }

            FileInfo f = new FileInfo(caminhotb_anexo);

            tb_anexo arqImagem = new tb_anexo();
            arqImagem.nome_arquivo=
                f.Name.Substring(f.Name.IndexOf("]") + 1);
            arqImagem.caminho = f.FullName;
            arqImagem.url = "~/PreVisualizacao/" + f.Name;
            arqImagem.data_cadastro = f.CreationTime;
            arqImagem.tamanho = (int)f.Length;
            arquivos.Add(arqImagem);
        }

        public static void RemoverArquivosListaPreVisualizacao(
            HttpSessionState session)
        {
            if (session["tb_anexosPreVisualizacao"] != null)
            {
                List<tb_anexo> arquivos =
                    (List<tb_anexo>)session["tb_anexosPreVisualizacao"];
                foreach (tb_anexo arqImagem in arquivos)
                {
                    File.Delete(arqImagem.caminho);
                }

                session["tb_anexosPreVisualizacao"] = null;
            }
        }
    }
}