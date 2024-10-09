using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BibliotecaNET8.Domain.Entities;

namespace BibliotecaNET8.DataAccess.Seeds;

/// <summary>
///     Configuración de la entidad "Categoria" para insertar datos iniciales en la Base de datos
/// </summary>
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
