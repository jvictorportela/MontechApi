using System.ComponentModel.DataAnnotations;

namespace MontechApi.Models;

public class Empresa
{
    [Key]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "O campo Nome é obrigatório")]
    [StringLength(200, MinimumLength = 1, ErrorMessage = "O campo Nome deve conter entre 1 e 200 caracteres")]
    public string Nome { get; set; }


    public Empresa()
    {
        Id = Guid.NewGuid();
    }
}
