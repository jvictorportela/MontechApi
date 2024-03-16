using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MontechApi.Data;
using MontechApi.Models;
using System.Net;

namespace MontechApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProdutoController : Controller
{
    private readonly MontechDbContext _context;

    public ProdutoController(MontechDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetProdutos()
    {
        try
        {
            var result = _context.Produto.ToList();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest($"Erro na listagem de produtos --> {ex.Message}");
        }
    }

    [HttpPost]
    [Authorize(Roles = "Gerente")]
    public async Task<IActionResult> PostProdutos([FromBody] Produto produto)
    {
        try
        {
            var maiorCodigoNoBanco = _context.Produto.Max(p => p.Codigo);
            if (maiorCodigoNoBanco != 0)
            {
                produto.Codigo = maiorCodigoNoBanco + 1;
                _context.Produto.Add(produto);
            }
            else
            {
                _context.Produto.Add(produto);
            }
            
            var valor = await _context.SaveChangesAsync();
            if (valor == 1)
            {
                return Ok($"Sucesso, produto incluído");
            }
            else
            {
                return BadRequest("Erro, produto não incluido.");
            }
        }
        catch (Exception ex)
        {
            return BadRequest($"Erro na inserção de produtos --> {ex.Message}");
        }
    }

    [HttpPut]
    [Authorize(Roles = "Gerente")]
    public async Task<IActionResult> PutProdutos([FromBody] Produto produto)
    {
        try
        {
            _context.Produto.Update(produto);
            var valor = await _context.SaveChangesAsync();
            if (valor == 1)
            {
                return Ok($"Sucesso, produto alterado com sucesso");
            }
            else
            {
                return BadRequest("Erro, produto não alterado.");
            }
        }
        catch (Exception ex)
        {
            return BadRequest($"Erro na alteração de produtos --> {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Gerente")]
    public async Task<IActionResult> DeleteProdutos([FromRoute] Guid id)
    {
        try
        {
            var produto = await _context.Produto.FindAsync(id);

            if (produto != null)
            {
                _context.Produto.Remove(produto);
                var valor = await _context.SaveChangesAsync();
                if (valor == 1)
                {
                    return Ok($"Sucesso, produto Excluido com sucesso");
                }
                else
                {
                    return BadRequest("Erro, produto não excluido.");
                }
            }
            else
            {
                return NotFound("Erro, produto não existe.");
            }
        }
        catch (Exception ex)
        {
            return BadRequest($"Erro na exclusão de produtos --> {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProdutoEspecifico([FromRoute] Guid id)
    {
        if(id == Guid.Empty)
        {
            return BadRequest("Não é possivel encontrar um id mal informado");
        }
        try
        {
            return Ok(await _context.Produto.FindAsync(id));
        }
        catch
        {
            return Problem("Por alguma razão, o item não foi encontrado", null, (int)HttpStatusCode.NotFound);
        }
    }


    [HttpGet("Pesquisa")]
    public async Task<IActionResult> GetProdutoPesquisa([FromQuery] string valor)
    {
        try
        {
            var lista = _context.Produto.ToList().Where(produto => produto.Nome.ToUpper().Contains(valor.ToUpper()));
            return Ok(lista);
        }
        catch (Exception ex)
        {
            return BadRequest($"Erro na pesquisa do produto --> {ex.Message}");
        }
    }

    [HttpGet("Paginacao")]
    public async Task<IActionResult> GetProdutoPaginacao([FromQuery] string valor, int skip, int take, bool ordemDecrescente)
    {
        try
        {
            var lista = _context.Produto.ToList().Where(produto => produto.Nome.ToUpper().Contains(valor.ToUpper())); //Impor categoria como "valor" para busca
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

            var paginacaoResponse = new PaginacaoResponse<Produto>(lista, quantidade, skip, take);

            return Ok(paginacaoResponse);
        }
        catch (Exception ex)
        {
            return BadRequest($"Erro na pesquisa do produto --> {ex.Message}");
        }
    }
}
