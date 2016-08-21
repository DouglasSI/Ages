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
    
    public partial class tb_fatura
    {
        public tb_fatura()
        {
            this.tb_compra = new HashSet<tb_compra>();
            this.tb_anexo = new HashSet<tb_anexo>();
        }
    
        public int id { get; set; }
        public int id_usuario { get; set; }
        public int id_forma_pagamento { get; set; }
        public int id_orcamento { get; set; }
        public Nullable<System.DateTime> data_pagamento { get; set; }
        public System.DateTime data_prevista { get; set; }
        public string titulo { get; set; }
        public string anotacao { get; set; }
        public string banco { get; set; }
        public string agencia { get; set; }
        public string conta { get; set; }
        public System.DateTime data_cadastro { get; set; }
        public decimal valor_inicial { get; set; }
        public decimal valor_pendente { get; set; }
        public bool is_aditivo { get; set; }
        public bool autorizado { get; set; }
        public bool encerrado { get; set; }
    
        public virtual ICollection<tb_compra> tb_compra { get; set; }
        public virtual tb_forma_pagamento tb_forma_pagamento { get; set; }
        public virtual tb_orcamento tb_orcamento { get; set; }
        public virtual tb_usuario tb_usuario { get; set; }
        public virtual ICollection<tb_anexo> tb_anexo { get; set; }
    }
}
