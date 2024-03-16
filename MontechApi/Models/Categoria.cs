using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MontechApi.Models;

public class Categoria
{
    [Key]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "O campo Nome é obrigatório")]
    [StringLength(200, MinimumLength = 3, ErrorMessage = "O campo Nome deve conter entre 3 e 200 caracteres")]
    public string Nome { get; set; }

    public Categoria()
    {
        Id = Guid.NewGuid();
    }
}
