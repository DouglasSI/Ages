//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class tb_endereco
    {
        public tb_endereco()
        {
            this.tb_campi = new HashSet<tb_campi>();
            this.tb_empresa = new HashSet<tb_empresa>();
            this.tb_mantenedora = new HashSet<tb_mantenedora>();
        }
    
        public int id { get; set; }
        public string logradouro { get; set; }
        public string numero { get; set; }
        public string complemento { get; set; }
        public string cep { get; set; }
        public string bairro { get; set; }
        public string municipio { get; set; }
        public string uf { get; set; }
    
        public virtual ICollection<tb_campi> tb_campi { get; set; }
        public virtual ICollection<tb_empresa> tb_empresa { get; set; }
        public virtual ICollection<tb_mantenedora> tb_mantenedora { get; set; }
    }
}
