//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class tb_status_projeto
    {
        public int id { get; set; }
        public string descricao { get; set; }

        public virtual ICollection<tb_projeto> tb_projeto { get; set; }
    }
}
