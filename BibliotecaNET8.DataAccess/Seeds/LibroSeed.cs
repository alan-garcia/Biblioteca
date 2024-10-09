using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BibliotecaNET8.Domain.Entities;

namespace BibliotecaNET8.DataAccess.Seeds;

/// <summary>
///     Configuración de la entidad "Libro" para insertar datos iniciales en la Base de datos
/// </summary>
public class LibroSeed : IEntityTypeConfiguration<Libro>
{
    public void Configure(EntityTypeBuilder<Libro> builder)
    {
        builder.HasData(
            new Libro { Id = 1, Titulo = "Los Pilares de la Tierra", ISBN = "087402", FechaPublicacion = new DateTime(2024, 9, 30), AutorId = 1, CategoriaId = 1, Imagen = null},
            new Libro { Id = 2, Titulo = "Moby Dick", ISBN = "4684", FechaPublicacion = new DateTime(2024, 9, 30), AutorId = 2, CategoriaId = 2, Imagen = null },
            new Libro { Id = 3, Titulo = "El resplandor", ISBN = "68453", FechaPublicacion = new DateTime(2024, 9, 30), AutorId = 3, CategoriaId = 3, Imagen = null},
            new Libro { Id = 4, Titulo = "La metamorfosis", ISBN = "867832", FechaPublicacion = new DateTime(2024, 9, 30), AutorId = 4, CategoriaId = 4, Imagen = null},
            new Libro { Id = 5, Titulo = "En busca del tiempo perdido", ISBN = "0275935", FechaPublicacion = new DateTime(2024, 9, 30), AutorId = 5, CategoriaId = 5, Imagen = null},
            new Libro { Id = 6, Titulo = "El Retrato de Dorian Gray", ISBN = "0275935", FechaPublicacion = new DateTime(2024, 9, 30), AutorId = 6, CategoriaId = 1, Imagen = null},
            new Libro { Id = 7, Titulo = "La vuelta al mundo en 80 días", ISBN = "0275935", FechaPublicacion = new DateTime(2024, 9, 30), AutorId = 7, CategoriaId = 2, Imagen = null},
            new Libro { Id = 8, Titulo = "Guerra y Paz", ISBN = "0275935", FechaPublicacion = new DateTime(2024, 9, 30), AutorId = 8, CategoriaId = 3, Imagen = null},
            new Libro { Id = 9, Titulo = "Frankenstein", ISBN = "0275935", FechaPublicacion = new DateTime(2024, 9, 30), AutorId = 9, CategoriaId = 4, Imagen = null},
            new Libro { Id = 10, Titulo = "Los miserables", ISBN = "0275935", FechaPublicacion = new DateTime(2024, 9, 30), AutorId = 10, CategoriaId = 5, Imagen = null},
            new Libro { Id = 11, Titulo = "El talento de Mr. Ripley", ISBN = "0275935", FechaPublicacion = new DateTime(2024, 9, 30), AutorId = 11, CategoriaId = 1, Imagen = null},
            new Libro { Id = 12, Titulo = "Romeo y Julieta", ISBN = "0275935", FechaPublicacion = new DateTime(2024, 9, 30), AutorId = 12, CategoriaId = 2, Imagen = null},
            new Libro { Id = 13, Titulo = "La casa de Bernarda Alba", ISBN = "0275935", FechaPublicacion = new DateTime(2024, 9, 30), AutorId = 13, CategoriaId = 3, Imagen = null},
            new Libro { Id = 14, Titulo = "Oliver Twist", ISBN = "0275935", FechaPublicacion = new DateTime(2024, 9, 30), AutorId = 14, CategoriaId = 4, Imagen = null},
            new Libro { Id = 15, Titulo = "El alquimista", ISBN = "0275935", FechaPublicacion = new DateTime(2024, 9, 30), AutorId = 15, CategoriaId = 5, Imagen = null }
        );
    }
}
