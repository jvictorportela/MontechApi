using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MontechApi.Data;
using MontechApi.Models;
using MontechApi.Services;

namespace MontechApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsuarioController : Controller
{
    private readonly MontechDbContext _context;
    private readonly TokenService _service;

    public UsuarioController(MontechDbContext context, TokenService service)
    {
        _context = context;
        _service = service;
    }

    [HttpPost]
    [Route("Login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] UsuarioLogin usuarioLogin)
    {
        var usuario = _context.Usuario.Where(x => x.Login == usuarioLogin.Login).FirstOrDefault();
        if (usuario == null)
        {
            return NotFound("Usuário inválido.");
        }

        var passwordHash = MD5Hash.CalcHash(usuarioLogin.Password);
               

        if (usuario.Password != passwordHash)
        {
            return BadRequest("Senha inválida.");
        }

        var token = _service.GenerateToken(usuario);

        usuario.Password = "";

        var result = new UsuarioResponse()
        {
            Usuario = usuario,
            Token = token
        };

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetUsuario()
    {
        try
        {
            var result = _context.Usuario.ToList();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest($"Erro na listagem de usuarios --> {ex.Message}");
        }
    }

    [HttpPost]
    [Authorize(Roles = "Gerente")]
    public async Task<IActionResult> PostUsuario([FromBody] Usuario usuario)
    {
        try
        {
            var usuariosComLoginIguais = _context.Usuario.Where(x => x.Login == usuario.Login).ToList();

            if (usuariosComLoginIguais.Count > 0)
            {
                return BadRequest("Erro, Informação de Login inválido, ou já existe.");
            }

            string passwordHash = MD5Hash.CalcHash(usuario.Password);
            usuario.Password = passwordHash;

            await _context.Usuario.AddAsync(usuario);
            var valor = await _context.SaveChangesAsync();
            if (valor == 1)
            {
                return Ok($"Sucesso, usuario incluído");
            }
            else
            {
                return BadRequest("Erro, usuario não incluido.");
            }
        }
        catch (Exception ex)
        {
            return BadRequest($"Erro na inserção de usuarios --> {ex.Message}");
        }
    }

    [HttpPut]
    [Authorize(Roles = "Gerente")]
    public async Task<IActionResult> PutUsuario([FromBody] Usuario usuario)
    {
        try
        {
            string passwordHash = MD5Hash.CalcHash(usuario.Password);
            usuario.Password = passwordHash;

            _context.Usuario.Update(usuario);
            var valor = await _context.SaveChangesAsync();
            if (valor == 1)
            {
                return Ok($"Sucesso, usuario alterado com sucesso");
            }
            else
            {
                return BadRequest("Erro, usuario não alterado.");
            }
        }
        catch (Exception ex)
        {
            return BadRequest($"Erro na alteração de usuarios --> {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Gerente")]
    public async Task<IActionResult> DeleteUsuario([FromRoute] Guid id)
    {
        try
        {
            var usuario = await _context.Usuario.FindAsync(id);

            if (usuario != null)
            {
                _context.Usuario.Remove(usuario);
                var valor = await _context.SaveChangesAsync();
                if (valor == 1)
                {
                    return Ok($"Sucesso, usuario Excluido com sucesso");
                }
                else
                {
                    return BadRequest("Erro, usuario não excluido");
                }
            }
            else
            {
                return NotFound("Erro, usuario não existe.");
            }
        }
        catch (Exception ex)
        {
            return BadRequest($"Erro na exclusão de usuarios --> {ex.Message}");
        }
    }
}
