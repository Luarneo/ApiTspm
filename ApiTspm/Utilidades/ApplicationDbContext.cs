using ApiTspm.Entidades;
using Microsoft.EntityFrameworkCore;

namespace ApiTspm.Utilidades
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions) : base(dbContextOptions)
        {
           
        }

        public DbSet<Anuncio> Anuncios { get; set; }

        public DbSet<Seccion> Secciones { get; set; }

        public DbSet<Galeria> Galerias { get; set; }

    }
}
 