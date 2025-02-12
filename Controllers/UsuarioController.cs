using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using TarefasBackEnd.Models;
using TarefasBackEnd.Models.ViewModels;
using TarefasBackEnd.Repositories;

namespace TarefasBackEnd.Controllers
{
    [ApiController]
    [Route("usuario")]
    public class UsuarioController : ControllerBase
    {
        [HttpPost]
        [Route("")]
        public IActionResult Create([FromBody]Usuario model, [FromServices] IUsuarioRepository repository)
        {
            if(!ModelState.IsValid)
                return BadRequest();
            repository.Create(model);

            return Ok();
        }
        [HttpPost]
        [Route("login")]
        public IActionResult Login ([FromBody] UsuarioLogin model, [FromServices] IUsuarioRepository repository)
        {
            if(model == null || string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Senha))
                
                return BadRequest("Bota o email e a senha amigão");
            
            if(!ModelState.IsValid)
                
                return BadRequest(ModelState);
            Usuario usuario;
            try
            {
                usuario = repository.Read(model.Email, model.Senha);
            }
            catch(KeyNotFoundException)
            {
                return Unauthorized("Algo errado, tente novamente");
            }
            if (usuario == null)
                return Unauthorized("Bota as info aí jovem");

            usuario.Senha = string.Empty;

            return Ok(new
            {
                usuario = usuario,
                token = GenerateToken(usuario)
            });
        }
        private string GenerateToken(Usuario usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            
            var key = Encoding.ASCII.GetBytes("NSeiTemQueGerarUmTokenComUmRnadom");


            var descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] {
                   new Claim(ClaimTypes.Name, usuario.Id.ToString()),          
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(descriptor);
            return tokenHandler.WriteToken(token);
            
        }
    }
}