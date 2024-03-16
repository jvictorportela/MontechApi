using System.ComponentModel.DataAnnotations;

namespace MontechApi.Models;

public class UsuarioLogin
{
    [Required(ErrorMessage = "O campo Login é obrigatório")]
    [StringLength(200, MinimumLength = 3, ErrorMessage = "O campo Login deve conter entre 3 e 200 caracteres")]
    public string Login { get; set; }

    [Required(ErrorMessage = "O campo Password é obrigatório")]
    [StringLength(20, MinimumLength = 3, ErrorMessage = "O campo Password deve conter entre 3 e 20 caracteres")]
    public string Password { get; set; }
}
