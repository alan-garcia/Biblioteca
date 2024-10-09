using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BibliotecaNET8.Domain.Entities;

namespace BibliotecaNET8.DataAccess.Seeds;

public class ClienteSeed : IEntityTypeConfiguration<Cliente>
{
    public void Configure(EntityTypeBuilder<Cliente> builder)
    {
        builder.HasData(
            new Cliente { Id = 1, Nombre = "José", Apellido = "Díaz Ramírez", Email = "test@test.com", Telefono = "999123456" },
            new Cliente { Id = 2, Nombre = "Antonio", Apellido = "García López", Email = "test2@test.com", Telefono = "999234567" },
            new Cliente { Id = 3, Nombre = "Pedro", Apellido = "Casado Mijares", Email = "test3@test.com", Telefono = "999345678" },
            new Cliente { Id = 4, Nombre = "Ana", Apellido = "Ramos Espinar", Email = "test4@test.com", Telefono = "999456789" },
            new Cliente { Id = 5, Nombre = "Laura", Apellido = "Escámez García", Email = "test5@test.com", Telefono = "999987654" },
            new Cliente { Id = 6, Nombre = "Sofía", Apellido = "González", Email = "test6@test.com", Telefono = "3468683" },
            new Cliente { Id = 7, Nombre = "Alejandro", Apellido = "Rodríguez", Email = "test7@test.com", Telefono = "79934563" },
            new Cliente { Id = 8, Nombre = "Laura", Apellido = "García", Email = "test8@test.com", Telefono = "" },
            new Cliente { Id = 9, Nombre = "David", Apellido = "López", Email = "test9@test.com", Telefono = "82663902" },
            new Cliente { Id = 10, Nombre = "María", Apellido = "Martín", Email = "test10@test.com", Telefono = "" }
        );
    }
}
