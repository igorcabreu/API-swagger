using Microsoft.EntityFrameworkCore;
using TarefasBackEnd.Models;

namespace TarefasBackEnd.Repositories
{
    public interface ITarefaRepository
    {
        List<Tarefa> Read(Guid id);
        Tarefa GetById(Guid id);
        void Create(Tarefa tarefa);
        void Delete(Guid id);
        void Update(Guid id, Tarefa tarefa);
    }

    public class TarefaRepository : ITarefaRepository
    {
        private readonly DataContext _context;
        public TarefaRepository(DataContext context)
        {
            _context = context;
        }
        public void Create(Tarefa tarefa)
        {
            tarefa.Id = Guid.NewGuid();
            _context.Tarefas.Add(tarefa);
            _context.SaveChanges();
        }

        public List<Tarefa> Read(Guid id)
        {
            return _context.Tarefas.Where(tarefa => tarefa.UsuarioId == id).ToList();
        }

        public Tarefa GetById(Guid id)
        {
            var tarefa = _context.Tarefas.Find(id);
            if (tarefa == null)
            { 
                throw new KeyNotFoundException("Tarefa não encontrada");
            } 
            return tarefa;
        }
        public void Update(Guid id, Tarefa tarefa)
        {
            var _tarefa = _context.Tarefas.Find(tarefa.Id);
            if (_tarefa == null)
            {
                throw new KeyNotFoundException("Tarefa não encontrada");
            }
            _tarefa.Nome = tarefa.Nome;
            _tarefa.Concluida = tarefa.Concluida;
            _context.Entry(_tarefa).State = EntityState.Modified;
            _context.SaveChanges();
        }
        public void Delete(Guid id)
        {
            var tarefa = _context.Tarefas.Find(id) ?? throw new KeyNotFoundException ("Tarefa não encontrada");
            _context.Entry(tarefa).State = EntityState.Deleted;
            _context.SaveChanges();
        }       
    }
}