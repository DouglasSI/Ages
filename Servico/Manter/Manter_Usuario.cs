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
            entidade = new db_agesEntities2();
        }
        private db_agesEntities2 entidade;
        public List<tb_usuario> obterUsuarios()
        {
            return entidade.tb_usuario.ToList();
        }
        public tb_usuario obterUsuario(string email, string senha)
        {
            return entidade.tb_usuario.Where(f => f.email.Equals(email) && f.senha.Equals(senha) && (bool)f.ativo).FirstOrDefault();
        }
        public tb_usuario obterUsuario(string email)
        {
            tb_usuario user = entidade.tb_usuario.Where(f => f.email.Equals(email)).FirstOrDefault();

            user.nome = user.nome.ToUpper() + " - " + user.perfil.ToUpper() + " - " + user.email;

            return user;
        }

        public tb_usuario obterUsuarioByid(int id)
        {
            tb_usuario user = entidade.tb_usuario.Where(f => f.id.Equals(id)).FirstOrDefault();

             return user;
        }
        public void cadastrar(tb_usuario usuario)
        {
            entidade.tb_usuario.Add(usuario);
            entidade.SaveChanges();
        }
        public void editar(tb_usuario usuario)
        {
            entidade = new db_agesEntities2();
            entidade.tb_usuario.Attach(usuario);
            var entry = entidade.Entry(usuario);
            entry.State = System.Data.Entity.EntityState.Modified;
           
            entidade.SaveChanges();   
        }
        
    }
}
