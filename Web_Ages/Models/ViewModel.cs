using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web_Ages.Models
{
    public  class ViewModel 
    {
          
        public  HttpPostedFileBase file { get; set; }

        public Servico.Manter_.tipo tipo { get; set; }

        public int id { get; set; }
        
        public  tb_anexo tb_anexo { get; set; }

        public string titulo { get; set; }

        public string anotacao { get; set; }

        public ViewModel()
        {
        
        }

    }
}