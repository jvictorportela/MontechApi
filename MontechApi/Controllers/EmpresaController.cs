using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MontechApi.Data;
using MontechApi.Models;
using System.Net;

namespace MontechApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EmpresaController : Controller
{
    private readonly MontechDbContext _context;

    public EmpresaController(MontechDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetEmpresa()
    {
        try
        {
            var result = _context.Empresa.ToList();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest($"Erro na listagem de Empresas --> {ex.Message}");
        }
    }

    [HttpPost]
    [Authorize(Roles = "Gerente")]
    public async Task<IActionResult> PostCategoria([FromBody] Empresa empresa)
    {
        try
        {
            await _context.Empresa.AddAsync(empresa);
            var valor = await _context.SaveChangesAsync();
            if (valor == 1)
            {
                return Ok($"Sucesso, empresa incluída");
            }
            else
            {
                return BadRequest("Erro, empresa não incluida.");
            }
        }
        catch (Exception ex)
        {
            return BadRequest($"Erro na inserção de empresas --> {ex.Message}");
        }
    }

    [HttpPut]
    [Authorize(Roles = "Gerente")]
    public async Task<IActionResult> PutEmpresa([FromBody] Empresa empresa)
    {
        try
        {
            _context.Empresa.Update(empresa);
            var valor = await _context.SaveChangesAsync();
            if (valor == 1)
            {
                return Ok($"Sucesso, empresa alterada com sucesso");
            }
            else
            {
                return BadRequest("Erro, empresa não alterada.");
            }
        }
        catch (Exception ex)
        {
            return BadRequest($"Erro na alteração de empresas --> {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Gerente")]
    public async Task<IActionResult> DeleteEmpresa([FromRoute] Guid id)
    {
        try
        {
            var empresa = await _context.Empresa.FindAsync(id);

            if (empresa != null)
            {
                _context.Empresa.Remove(empresa);
                var valor = await _context.SaveChangesAsync();
                if (valor == 1)
                {
                    return Ok($"Sucesso, empresa Excluida com sucesso");
                }
                else
                {
                    return BadRequest("Erro, empresa não excluidoa");
                }
            }
            else
            {
                return NotFound("Erro, empresa não existe.");
            }
        }
        catch (Exception ex)
        {
            return BadRequest($"Erro na exclusão de empresas --> {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetEmpresaEspecifica([FromRoute] Guid id)
    {
        if (id == Guid.Empty)
        {
            return BadRequest("Não é possivel encontrar um id mal informado");
        }
        try
        {
            return Ok(await _context.Empresa.FindAsync(id));
        }
        catch
        {
            return Problem("Por alguma razão, o item não foi encontrado", null, (int)HttpStatusCode.NotFound);
        }
    }

    [HttpGet("Pesquisa")]
    public async Task<IActionResult> GetEmpresaPesquisa([FromQuery] string valor)
    {
        try
        {
            var lista = _context.Empresa.ToList().Where(empresa => empresa.Nome.ToUpper().Contains(valor.ToUpper()));
            return Ok(lista);
        }
        catch (Exception ex)
        {
            return BadRequest($"Erro na pesquisa da empresa --> {ex.Message}");
        }
    }

    [HttpGet("Paginacao")]
    public async Task<IActionResult> GetEmpresaPaginacao([FromQuery] string valor, int skip, int take, bool ordemDecrescente)
    {
        try
        {
            var lista = _context.Empresa.ToList().Where(empresa => empresa.Nome.ToUpper().Contains(valor.ToUpper())); 
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

            var paginacaoResponse = new PaginacaoResponse<Empresa>(lista, quantidade, skip, take);

            return Ok(paginacaoResponse);
        }
        catch (Exception ex)
        {
            return BadRequest($"Erro na pesquisa da empresa --> {ex.Message}");
        }
    }
}
