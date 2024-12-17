using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TarefasBackEnd.Models;
using TarefasBackEnd.Repositories;

namespace TarefasBackEnd.Controllers
{
    [Authorize]
    [ApiController]
    [Route("tarefa")]
    public class TarefaController : ControllerBase
    {
        [HttpGet("{id}")]
        public IActionResult GetById(string id, [FromServices] ITarefaRepository repository)
        {
            if(!Guid.TryParse(id, out var guidId))
                return BadRequest("ID não existe");
            try
            {
                var tarefa = repository.GetById(guidId);
                return Ok(tarefa);
            }
            catch(KeyNotFoundException)
            {
                return NotFound("Tarefa não encontrada");
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Read([FromServices] ITarefaRepository repository)
        {
            var id = new Guid(User.Identity.Name);
            var tarefas = repository.Read(id);
            return Ok(tarefas);
        }
        [HttpPost]
        public IActionResult Create([FromBody] Tarefa model, [FromServices] ITarefaRepository repository)
        {
            if(!ModelState.IsValid)
                return BadRequest();
            
            model.UsuarioId =new Guid (User.Identity.Name);
            repository.Create(model);

            return Ok();
        }
        [HttpPut("id")]
        public IActionResult Update(string id,[FromBody] Tarefa model, [FromServices] ITarefaRepository repository)
        {
            if(!ModelState.IsValid)
                return BadRequest();
            if(!Guid.TryParse(id, out var guidId))
                return BadRequest("Id Inválido");
            try
            {
                model.Id = guidId;
                repository.Update(guidId, model);
                return Ok(model);
            }
            catch(KeyNotFoundException)
            {
                return NotFound("Tarefa não encontrada");
            }
        }
        [HttpDelete("id")]
        public IActionResult Delete(string id, [FromServices] ITarefaRepository repository)
        {

            repository.Delete(new Guid(id));

            return Ok();
        }
    }
    
}