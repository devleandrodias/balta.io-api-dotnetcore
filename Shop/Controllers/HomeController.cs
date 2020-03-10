using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shop.Data;
using Shop.Models;

namespace Shop.Controllers
{
  [Route("v1"), ApiController]
  public class HomeController : Controller
  {
    [HttpGet]
    public async Task<ActionResult<dynamic>> Get([FromServices] DataContext context)
    {
      User maneger = new User
      {
        Id = 1,
        Username = "Leandro",
        Password = "bemfica",
        Role = "maneger"
      };

      User employee = new User
      {
        Id = 1,
        Username = "Leandro",
        Password = "bemfica",
        Role = "employee"
      };

      Category category = new Category
      {
        Id = 1,
        Title = "Eletrônicos"
      };

      Product product = new Product
      {
        Id = 1,
        Title = "Mac BookAir 2017",
        Price = 15789,
        Description = "Notebook da apple modelo MacBook Air 2017",
        Category = category
      };

      context.Users.Add(maneger);
      context.Users.Add(employee);
      context.Categories.Add(category);
      context.Products.Add(product);

      await context.SaveChangesAsync();

      return Ok(new
      {
        message = "Dados configurados com sucesso!"
      });
    }
  }
}
