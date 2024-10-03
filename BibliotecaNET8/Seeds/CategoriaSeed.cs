using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BibliotecaNET8.Models;

namespace BibliotecaNET8.Seeds;

public class CategoriaSeed : IEntityTypeConfiguration<Categoria>
{
    public void Configure(EntityTypeBuilder<Categoria> builder)
    {
        builder.HasData(
            new Categoria { Id = 1, Nombre = "Acción" },
            new Categoria { Id = 2, Nombre = "Suspenso" },
            new Categoria { Id = 3, Nombre = "Romance" },
            new Categoria { Id = 4, Nombre = "Drama" },
            new Categoria { Id = 5, Nombre = "Terror" }
        );
    }
}
