using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;


namespace Servico.Manter
{
    public class Manter_Usuario
    {
        public Manter_Usuario()
        {
            entidade = new Entities();
        }
        private Entities entidade;
        public List<tb_usuario> obterUsuarios()
        {
            return entidade.tb_usuario.ToList();
        }
        public tb_usuario obterUsuario(string nome, string senha)
        {
            return entidade.tb_usuario.Where(f => f.nome.Equals(nome) && f.senha.Equals(senha)).FirstOrDefault();
        }
        public void cadastrar(tb_usuario usuario)
        {
            entidade.tb_usuario.Add(usuario);   
        }
        public void editar(tb_usuario usuario)
        {
            entidade = new Entities();
            entidade.tb_usuario.Attach(usuario);
            var entry = entidade.Entry(usuario);
            entry.State = System.Data.Entity.EntityState.Modified;
            entry.Property(e => e.id).IsModified = false;
            entidade.SaveChanges();   
        }
    }
}
