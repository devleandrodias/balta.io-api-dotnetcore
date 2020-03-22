using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;

namespace Shop.Controllers
{
  [Route("v1/products"), ApiController]
  public class ProductController : Controller
  {
    [HttpGet]
    public async Task<ActionResult<List<Product>>> Get([FromServices] DataContext context)
    {
      List<Product> products = await context
        .Products
        .Include(x => x.Category)
        .AsNoTracking()
        .ToListAsync();

      return Ok(products);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetById([FromRoute] int id, [FromServices] DataContext context)
    {
      Product Product = await context
      .Products
      .Include(x => x.Category)
      .AsNoTracking()
      .FirstOrDefaultAsync();

      return Ok(Product);
    }

    [HttpPost]
    public async Task<ActionResult<List<Product>>> Post([FromBody] Product model, [FromServices] DataContext context)
    {
      if (!ModelState.IsValid)
        return BadRequest(ModelState);

      try
      {
        context.Products.Add(model);
        await context.SaveChangesAsync();
        return Ok(model);
      }

      catch (System.Exception)
      {
        return BadRequest(new { message = "Não foi possível cadastrar o produto." });
      }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<List<Product>>> Put([FromRoute] int id, [FromBody] Product model, [FromServices] DataContext context)
    {
      if (id != model.Id)
        return NotFound(new { message = "Produto não encontrado" });

      if (!ModelState.IsValid)
        return BadRequest(ModelState);

      try
      {
        context.Entry<Product>(model).State = EntityState.Modified;
        await context.SaveChangesAsync();
        return Ok(model);
      }

      catch (DbUpdateConcurrencyException)
      {
        return BadRequest(new { message = "Este registro já foi atualizado." });
      }

      catch (Exception)
      {
        return BadRequest(new { message = "Não foi possível atualizar o produto." });
      }
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<List<Product>>> Delete([FromRoute] int id, [FromServices] DataContext context)
    {
      Product product = await context.Products.FirstOrDefaultAsync(x => x.Id == id);

      if (product == null)
        return NotFound(new { message = "Produto não encontrado" });

      try
      {
        context.Products.Remove(product);
        await context.SaveChangesAsync();
        return Ok(new { message = "Produto removido com sucesso." });
      }

      catch (System.Exception)
      {
        return BadRequest(new { message = "Produto não encontrado" });
      }
    }

    [HttpGet("categories/{id:int}")]
    public async Task<ActionResult<List<Product>>> GetByCategory([FromRoute] int id, [FromServices] DataContext context)
    {
      List<Product> products = await context
         .Products
         .Include(x => x.Category)
         .AsNoTracking()
         .Where(x => x.CategoryId == id)
         .ToListAsync();

      return products;
    }
  }
}
