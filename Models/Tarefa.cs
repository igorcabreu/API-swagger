using System.ComponentModel.DataAnnotations;
using TarefasBackEnd.Enums;

namespace TarefasBackEnd.Models
{
    public class Tarefa
    {
        public Guid Id { get; set;}
        public Guid UsuarioId { get; set; }
        [Required]
        public string? Nome { get; set;}
        public StatusTarefa Concluida { get; set; }
    }
}