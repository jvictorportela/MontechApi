using System.ComponentModel.DataAnnotations;

namespace MontechApi.Models;

public class Usuario
{
    [Key]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "O campo Nome é obrigatório")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "O campo Nome deve conter entre 3 e 100 caracteres")]
    public string Nome { get; set; }

    [Required(ErrorMessage = "O campo Login é obrigatório")]
    [StringLength(200, MinimumLength = 3, ErrorMessage = "O campo Login deve conter entre 3 e 200 caracteres")]
    public string Login { get; set; }

    [Required(ErrorMessage = "O campo Password é obrigatório")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "O campo Password deve conter entre 3 e 100 caracteres")]
    public string Password { get; set; }

    [Required(ErrorMessage = "O campo Função é obrigatório")]
    [StringLength(20, MinimumLength = 2, ErrorMessage = "O campo Função deve conter entre 2 e 20 caracteres")]
    public string Funcao { get; set; }

    public Usuario()
    {
        Id = Guid.NewGuid();
    }
}
