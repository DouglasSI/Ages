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

    public partial class tb_usuario
    {
        public tb_usuario()
        {
            this.tb_anexo = new HashSet<tb_anexo>();
            this.tb_fatura = new HashSet<tb_fatura>();
            this.tb_orcamento = new HashSet<tb_orcamento>();
            this.tb_projeto = new HashSet<tb_projeto>();
            this.tb_servico = new HashSet<tb_servico>();
        }
        public int id { get; set; }
        public string nome { get; set; }
        public string senha { get; set; }
        public int id_funcao { get; set; }
        public string email { get; set; }
        public bool ativo { get; set; }
        public string perfil { get; set; }
        public string sobrenome { get; set; }
        public virtual ICollection<tb_anexo> tb_anexo { get; set; }
        public virtual ICollection<tb_fatura> tb_fatura { get; set; }
        public virtual tb_funcao tb_funcao { get; set; }
        public virtual ICollection<tb_orcamento> tb_orcamento { get; set; }
        public virtual ICollection<tb_projeto> tb_projeto { get; set; }
        public virtual ICollection<tb_servico> tb_servico { get; set; }
    }
}
