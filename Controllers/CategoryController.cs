using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;

namespace Shop.Controllers
{
  [Route("v1/categories"), ApiController]
  public class CategoryController : Controller
  {
    [HttpGet, AllowAnonymous, ResponseCache(VaryByHeader = "User-Agent", Location = ResponseCacheLocation.Any, Duration = 30)]
    // Caso startup tiver cache e método não ter cache
    // [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<ActionResult<List<Category>>> Get([FromServices] DataContext context)
    {
      List<Category> categories = await context.Categories.AsNoTracking().ToListAsync();

      return Ok(categories);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Category>> GetById([FromRoute] int id, [FromServices] DataContext context)
    {
      Category category = await context.Categories.AsNoTracking().FirstOrDefaultAsync();

      return Ok(category);
    }

    [HttpPost]
    public async Task<ActionResult<List<Category>>> Post([FromBody] Category model, [FromServices] DataContext context)
    {
      if (!ModelState.IsValid)
        return BadRequest(ModelState);

      try
      {
        context.Categories.Add(model);
        await context.SaveChangesAsync();
        return Ok(model);
      }

      catch (System.Exception)
      {
        return BadRequest(new { message = "Não foi possível cadastrar a categoria." });
      }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<List<Category>>> Put([FromRoute] int id, [FromBody] Category model, [FromServices] DataContext context)
    {
      if (id != model.Id)
        return NotFound(new { message = "Categoria não encontrada" });

      if (!ModelState.IsValid)
        return BadRequest(ModelState);

      try
      {
        context.Entry<Category>(model).State = EntityState.Modified;
        await context.SaveChangesAsync();
        return Ok(model);
      }

      catch (DbUpdateConcurrencyException)
      {
        return BadRequest(new { message = "Este registro já foi atualizado." });
      }

      catch (Exception)
      {
        return BadRequest(new { message = "Não foi possível atualizar a categoria." });
      }
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<List<Category>>> Delete([FromRoute] int id, [FromServices] DataContext context)
    {
      Category category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);

      if (category == null)
        return NotFound(new { message = "Categori não encontrada" });

      try
      {
        context.Categories.Remove(category);
        await context.SaveChangesAsync();
        return Ok(new { message = "Categoria removida com sucesso." });
      }
      catch (System.Exception)
      {
        return BadRequest(new { message = "Categoria não encontrada" });
      }
    }
  }
}
