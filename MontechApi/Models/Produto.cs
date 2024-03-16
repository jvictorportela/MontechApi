using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MontechApi.Models;

public class Produto
{
    [Key]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "O campo Nome é obrigatório")]
    [StringLength(200, MinimumLength = 3, ErrorMessage = "O campo Nome deve conter entre 3 e 200 caracteres")]
    public string Nome { get; set; }

    [Required(ErrorMessage = "O campo Código é obrigatório")]
    public int Codigo { get; set; }

    [StringLength(750, MinimumLength = 3, ErrorMessage = "O campo Descricao deve conter entre 3 e 750 caracteres")]
    public string? Descricao { get; set; }

    [Required(ErrorMessage = "O campo Data da compra é obrigatório")]
    public DateTime DataCompra { get; set; }

    [Required(ErrorMessage = "O campo Valor da compra é obrigatório")]
    public decimal ValorCompra { get; set; }

    public decimal? ValorDeMercado { get; set; }

    [Required(ErrorMessage = "O campo Categoria é obrigatório")]
    public string CategoriaProd { get; set; }

    [JsonIgnore]
    public Categoria? Categoria { get; set; }

    public Produto()
    {
        Id = Guid.NewGuid();
        DataCompra = DateTime.UtcNow;
    }
}
