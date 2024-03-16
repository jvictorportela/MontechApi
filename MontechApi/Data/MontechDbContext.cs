using Microsoft.EntityFrameworkCore;
using MontechApi.Models;

namespace MontechApi.Data;

public class MontechDbContext : DbContext
{
    public MontechDbContext(DbContextOptions<MontechDbContext> options) : base(options)
    {
        
    }

    public DbSet<Produto> Produto { get; set; }
    public DbSet<Categoria> Categoria { get; set; }
    public DbSet<Empresa> Empresa { get; set; }
    public DbSet<Usuario> Usuario { get; set; }
}
