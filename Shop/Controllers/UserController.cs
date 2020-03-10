using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;
using Shop.Services;

namespace Shop.Controllers
{
  [Route("v1/users"), ApiController]
  public class UserController : Controller
  {
    [HttpPost, AllowAnonymous]
    public async Task<ActionResult<User>> Post([FromServices] DataContext context, [FromBody] User model)
    {
      if (!ModelState.IsValid)
        return BadRequest(ModelState);

      try
      {
        model.Role = "employee";
        context.Users.Add(model);
        await context.SaveChangesAsync();
        model.Password = "";
        return model;
      }

      catch (System.Exception)
      {
        return BadRequest(new { message = "Não foi possível criar um usuários" });
      }
    }

    [HttpPost("login")]
    public async Task<ActionResult<dynamic>> Authenticate([FromServices] DataContext context, [FromBody] User model)
    {
      User user = await context
        .Users
        .AsNoTracking()
        .Where(x => x.Username == model.Username && x.Password == model.Password)
        .FirstOrDefaultAsync();

      if (user == null)
        return NotFound(new { message = "Usuário ou senha inválidos" });

      string token = TokenService.GenerateToken(user);

      return new
      {
        id = user.Id,
        username = user.Username,
        role = user.Role,
        token = token
      };
    }

    [HttpGet, Authorize(Roles = "manager")]
    public async Task<ActionResult<List<User>>> Get([FromServices] DataContext context)
    {
      return await context.Users.AsNoTracking().ToListAsync();
    }
  }
}
