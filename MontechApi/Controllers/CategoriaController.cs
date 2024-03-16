using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MontechApi.Data;
using MontechApi.Models;
using System.Net;

namespace MontechApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CategoriaController : Controller
{
    private readonly MontechDbContext _context;

    public CategoriaController(MontechDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetCategoria()
    {
        try
        {
            var result = _context.Categoria.ToList();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest($"Erro na listagem de categorias --> {ex.Message}");
        }
    }

    [HttpPost]
    [Authorize(Roles = "Gerente")]
    public async Task<IActionResult> PostCategoria([FromBody] Categoria categoria)
    {
        try
        {
            await _context.Categoria.AddAsync(categoria);
            var valor = await _context.SaveChangesAsync();
            if (valor == 1)
            {
                return Ok($"Sucesso, categoria incluída");
            }
            else
            {
                return BadRequest("Erro, categoria não incluida.");
            }
        }
        catch (Exception ex)
        {
            return BadRequest($"Erro na inserção de categorias --> {ex.Message}");
        }
    }

    [HttpPut]
    [Authorize(Roles = "Gerente")]
    public async Task<IActionResult> PutCategoria([FromBody] Categoria categoria)
    {
        try
        {
            _context.Categoria.Update(categoria);
            var valor = await _context.SaveChangesAsync();
            if (valor == 1)
            {
                return Ok($"Sucesso, categoria alterada com sucesso");
            }
            else
            {
                return BadRequest("Erro, categoria não alterada.");
            }
        }
        catch (Exception ex)
        {
            return BadRequest($"Erro na alteração de categorias --> {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Gerente")]
    public async Task<IActionResult> DeleteCategoria([FromRoute] Guid id)
    {
        try
        {
            var categoria = await _context.Categoria.FindAsync(id);

            if (categoria != null)
            {
                _context.Categoria.Remove(categoria);
                var valor = await _context.SaveChangesAsync();
                if (valor == 1)
                {
                    return Ok($"Sucesso, categoria Excluida com sucesso");
                }
                else
                {
                    return BadRequest("Erro, categoria não excluida");
                }
            }
            else
            {
                return NotFound("Erro, categoria não existe.");
            }
        }
        catch (Exception ex)
        {
            return BadRequest($"Erro na exclusão de categorias --> {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCategoriaEspecifica([FromRoute] Guid id)
    {
        if (id == Guid.Empty)
        {
            return BadRequest("Não é possivel encontrar um id mal informado");
        }
        try
        {
            return Ok(await _context.Categoria.FindAsync(id));
        }
        catch
        {
            return Problem("Por alguma razão, o item não foi encontrado", null, (int)HttpStatusCode.NotFound);
        }
    }

    [HttpGet("Pesquisa")]
    public async Task<IActionResult> GetCategoriaPesquisa([FromQuery] string valor)
    {
        try
        {
            var lista = _context.Categoria.ToList().Where(categoria => categoria.Nome.ToUpper().Contains(valor.ToUpper()));
            return Ok(lista);
        }
        catch (Exception ex)
        {
            return BadRequest($"Erro na pesquisa da categoria --> {ex.Message}");
        }
    }

    [HttpGet("Paginacao")]
    public async Task<IActionResult> GetCategoriaPaginacao([FromQuery] string valor, int skip, int take, bool ordemDecrescente)
    {
        try
        {
            var lista = _context.Categoria.ToList().Where(categoria => categoria.Nome.ToUpper().Contains(valor.ToUpper())); 
            if (ordemDecrescente)
            {
                lista = lista.OrderBy(o => o.Nome).Reverse();
            }
            else
            {
                lista = lista.OrderBy(o => o.Nome);
            }

            var quantidade = lista.Count();

            lista = lista.Skip(skip)
                .Take(take)
                .ToList();

            var paginacaoResponse = new PaginacaoResponse<Categoria>(lista, quantidade, skip, take);

            return Ok(paginacaoResponse);
        }
        catch (Exception ex)
        {
            return BadRequest($"Erro na pesquisa da categoria --> {ex.Message}");
        }
    }
}
