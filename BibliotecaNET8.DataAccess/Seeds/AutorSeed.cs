using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BibliotecaNET8.Domain.Entities;

namespace BibliotecaNET8.DataAccess.Seeds;

public class AutorSeed : IEntityTypeConfiguration<Autor>
{
    public void Configure(EntityTypeBuilder<Autor> builder)
    {
        builder.HasData(
            new Autor { Id = 1, Nombre = "Mary", Apellido = "Shelley", FechaNacimiento = DateTime.Today },
            new Autor { Id = 2, Nombre = "Leon", Apellido = "Tolstoi", FechaNacimiento = DateTime.Today },
            new Autor { Id = 3, Nombre = "Julio", Apellido = "Verne", FechaNacimiento = DateTime.Today },
            new Autor { Id = 4, Nombre = "Oscar", Apellido = "Wilde", FechaNacimiento = DateTime.Today },
            new Autor { Id = 5, Nombre = "Virginia", Apellido = "Woolf", FechaNacimiento = DateTime.Today },
            new Autor { Id = 6, Nombre = "Edgar", Apellido = "Allan Poe", FechaNacimiento = DateTime.Today },
            new Autor { Id = 7, Nombre = "Jane", Apellido = "Austen", FechaNacimiento = DateTime.Today },
            new Autor { Id = 8, Nombre = "Miguel de Cervantes", Apellido = "Saavedra", FechaNacimiento = DateTime.Today },
            new Autor { Id = 9, Nombre = "Agatha", Apellido = "Chistie", FechaNacimiento = DateTime.Today },
            new Autor { Id = 10, Nombre = "Paulo", Apellido = "Coelho", FechaNacimiento = DateTime.Today },
            new Autor { Id = 11, Nombre = "Charles ", Apellido = "Dickens", FechaNacimiento = DateTime.Today },
            new Autor { Id = 12, Nombre = "Ken", Apellido = "Follet", FechaNacimiento = DateTime.Today },
            new Autor { Id = 13, Nombre = "Federico", Apellido = "García Lorca", FechaNacimiento = DateTime.Today },
            new Autor { Id = 14, Nombre = "Gabriel", Apellido = "García Márquez", FechaNacimiento = DateTime.Today },
            new Autor { Id = 15, Nombre = "Patricia", Apellido = "Highsmith", FechaNacimiento = DateTime.Today },
            new Autor { Id = 16, Nombre = "Víctor", Apellido = "Hugo", FechaNacimiento = DateTime.Today },
            new Autor { Id = 17, Nombre = "James", Apellido = "Joyce", FechaNacimiento = DateTime.Today },
            new Autor { Id = 18, Nombre = "Franz", Apellido = "Kafka", FechaNacimiento = DateTime.Today },
            new Autor { Id = 19, Nombre = "Stephen", Apellido = "King", FechaNacimiento = DateTime.Today },
            new Autor { Id = 20, Nombre = "Ernest", Apellido = "Hemingway", FechaNacimiento = DateTime.Today },
            new Autor { Id = 21, Nombre = "Félix", Apellido = "Lope de Vega", FechaNacimiento = DateTime.Today },
            new Autor { Id = 22, Nombre = "Herman", Apellido = "Melville", FechaNacimiento = DateTime.Today },
            new Autor { Id = 23, Nombre = "Pablo", Apellido = "Neruda", FechaNacimiento = DateTime.Today },
            new Autor { Id = 24, Nombre = "Marcel", Apellido = "Proust", FechaNacimiento = DateTime.Today },
            new Autor { Id = 25, Nombre = "William", Apellido = "Shakespeare", FechaNacimiento = DateTime.Today }
        );
    }
}
