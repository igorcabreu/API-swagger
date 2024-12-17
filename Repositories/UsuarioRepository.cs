using TarefasBackEnd.Models;

namespace TarefasBackEnd.Repositories
{
    public interface IUsuarioRepository
    {
        Usuario Read(string email, string senha);
        void Create(Usuario usuario);
    }
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly DataContext _context;
        public UsuarioRepository(DataContext context)
        {
            _context = context;
        }
        public void Create(Usuario usuario)
        {
            usuario.Id= Guid.NewGuid();
            _context.Usuarios.Add(usuario);
            _context.SaveChanges();
        }

        public Usuario Read(string email, string senha)
        {
            if(string.IsNullOrEmpty(email) || string.IsNullOrEmpty(senha))
            {
                throw new ArgumentException("Põem email e senha aí");
            }
            var usuario = _context.Usuarios.FirstOrDefault(
                usuario => usuario.Email == email && usuario.Senha == senha
            );
            if(usuario == null)
            {
                throw new KeyNotFoundException("Usuario não encontrado");
            }
            return usuario;
        }
    }
}