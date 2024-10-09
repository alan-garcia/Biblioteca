using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BibliotecaNET8.Domain.Entities;

namespace BibliotecaNET8.DataAccess.Seeds;

/// <summary>
///     Configuración de la entidad "Prestamo" para insertar datos iniciales en la Base de datos
/// </summary>
public class PrestamoSeed : IEntityTypeConfiguration<Prestamo>
{
    public void Configure(EntityTypeBuilder<Prestamo> builder)
    {
        builder.HasData(
            new Prestamo { Id = 1, FechaPrestamo = new DateTime(2024, 9, 30), FechaDevolucion = new DateTime(2024, 9, 30), ClienteId = 1, LibroId = 1},
            new Prestamo { Id = 2, FechaPrestamo = new DateTime(2024, 9, 30), FechaDevolucion = new DateTime(2024, 9, 30), ClienteId = 2, LibroId = 2},
            new Prestamo { Id = 3, FechaPrestamo = new DateTime(2024, 9, 30), FechaDevolucion = new DateTime(2024, 9, 30), ClienteId = 3, LibroId = 3},
            new Prestamo { Id = 4, FechaPrestamo = new DateTime(2024, 9, 30), FechaDevolucion = new DateTime(2024, 9, 30), ClienteId = 4, LibroId = 4},
            new Prestamo { Id = 5, FechaPrestamo = new DateTime(2024, 9, 30), FechaDevolucion = new DateTime(2024, 9, 30), ClienteId = 5, LibroId = 5},
            new Prestamo { Id = 6, FechaPrestamo = new DateTime(2024, 9, 30), FechaDevolucion = new DateTime(2024, 9, 30), ClienteId = 6, LibroId = 6},
            new Prestamo { Id = 7, FechaPrestamo = new DateTime(2024, 9, 30), FechaDevolucion = new DateTime(2024, 9, 30), ClienteId = 7, LibroId = 7},
            new Prestamo { Id = 8, FechaPrestamo = new DateTime(2024, 9, 30), FechaDevolucion = new DateTime(2024, 9, 30), ClienteId = 8, LibroId = 8},
            new Prestamo { Id = 9, FechaPrestamo = new DateTime(2024, 9, 30), FechaDevolucion = new DateTime(2024, 9, 30), ClienteId = 9, LibroId = 9},
            new Prestamo { Id = 10, FechaPrestamo = new DateTime(2024, 9, 30), FechaDevolucion = new DateTime(2024, 9, 30), ClienteId = 10, LibroId = 10}
        );
    }
}
