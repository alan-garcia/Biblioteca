using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BibliotecaNET8.Migrations
{
    /// <inheritdoc />
    public partial class CreateBibliotecaTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Autores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Apellido = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FechaNacimiento = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Autores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Apellido = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Libros",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    ISBN = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: true),
                    FechaPublicacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Imagen = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    AutorId = table.Column<int>(type: "int", nullable: false),
                    CategoriaId = table.Column<int>(type: "int", nullable: false),
                    ClienteId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Libros", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Libros_Autores_AutorId",
                        column: x => x.AutorId,
                        principalTable: "Autores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Libros_Categorias_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categorias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Libros_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Prestamo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaPrestamo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaDevolucion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LibroId = table.Column<int>(type: "int", nullable: false),
                    ClienteId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prestamo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Prestamo_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Prestamo_Libros_LibroId",
                        column: x => x.LibroId,
                        principalTable: "Libros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Autores",
                columns: new[] { "Id", "Apellido", "FechaNacimiento", "Nombre" },
                values: new object[,]
                {
                    { 1, "Shelley", new DateTime(2024, 10, 2, 0, 0, 0, 0, DateTimeKind.Local), "Mary" },
                    { 2, "Tolstoi", new DateTime(2024, 10, 2, 0, 0, 0, 0, DateTimeKind.Local), "Leon" },
                    { 3, "Verne", new DateTime(2024, 10, 2, 0, 0, 0, 0, DateTimeKind.Local), "Julio" },
                    { 4, "Wilde", new DateTime(2024, 10, 2, 0, 0, 0, 0, DateTimeKind.Local), "Oscar" },
                    { 5, "Woolf", new DateTime(2024, 10, 2, 0, 0, 0, 0, DateTimeKind.Local), "Virginia" },
                    { 6, "Allan Poe", new DateTime(2024, 10, 2, 0, 0, 0, 0, DateTimeKind.Local), "Edgar" },
                    { 7, "Austen", new DateTime(2024, 10, 2, 0, 0, 0, 0, DateTimeKind.Local), "Jane" },
                    { 8, "Saavedra", new DateTime(2024, 10, 2, 0, 0, 0, 0, DateTimeKind.Local), "Miguel de Cervantes" },
                    { 9, "Chistie", new DateTime(2024, 10, 2, 0, 0, 0, 0, DateTimeKind.Local), "Agatha" },
                    { 10, "Coelho", new DateTime(2024, 10, 2, 0, 0, 0, 0, DateTimeKind.Local), "Paulo" },
                    { 11, "Dickens", new DateTime(2024, 10, 2, 0, 0, 0, 0, DateTimeKind.Local), "Charles " },
                    { 12, "Follet", new DateTime(2024, 10, 2, 0, 0, 0, 0, DateTimeKind.Local), "Ken" },
                    { 13, "García Lorca", new DateTime(2024, 10, 2, 0, 0, 0, 0, DateTimeKind.Local), "Federico" },
                    { 14, "García Márquez", new DateTime(2024, 10, 2, 0, 0, 0, 0, DateTimeKind.Local), "Gabriel" },
                    { 15, "Highsmith", new DateTime(2024, 10, 2, 0, 0, 0, 0, DateTimeKind.Local), "Patricia" },
                    { 16, "Hugo", new DateTime(2024, 10, 2, 0, 0, 0, 0, DateTimeKind.Local), "Víctor" },
                    { 17, "Joyce", new DateTime(2024, 10, 2, 0, 0, 0, 0, DateTimeKind.Local), "James" },
                    { 18, "Kafka", new DateTime(2024, 10, 2, 0, 0, 0, 0, DateTimeKind.Local), "Franz" },
                    { 19, "King", new DateTime(2024, 10, 2, 0, 0, 0, 0, DateTimeKind.Local), "Stephen" },
                    { 20, "Hemingway", new DateTime(2024, 10, 2, 0, 0, 0, 0, DateTimeKind.Local), "Ernest" },
                    { 21, "Lope de Vega", new DateTime(2024, 10, 2, 0, 0, 0, 0, DateTimeKind.Local), "Félix" },
                    { 22, "Melville", new DateTime(2024, 10, 2, 0, 0, 0, 0, DateTimeKind.Local), "Herman" },
                    { 23, "Neruda", new DateTime(2024, 10, 2, 0, 0, 0, 0, DateTimeKind.Local), "Pablo" },
                    { 24, "Proust", new DateTime(2024, 10, 2, 0, 0, 0, 0, DateTimeKind.Local), "Marcel" },
                    { 25, "Shakespeare", new DateTime(2024, 10, 2, 0, 0, 0, 0, DateTimeKind.Local), "William" }
                });

            migrationBuilder.InsertData(
                table: "Categorias",
                columns: new[] { "Id", "Nombre" },
                values: new object[,]
                {
                    { 1, "Acción" },
                    { 2, "Suspenso" },
                    { 3, "Romance" },
                    { 4, "Drama" },
                    { 5, "Terror" }
                });

            migrationBuilder.InsertData(
                table: "Clientes",
                columns: new[] { "Id", "Apellido", "Email", "Nombre", "Telefono" },
                values: new object[,]
                {
                    { 1, "Díaz Ramírez", "test@test.com", "José", "999123456" },
                    { 2, "García López", "test2@test.com", "Antonio", "999234567" },
                    { 3, "Casado Mijares", "test3@test.com", "Pedro", "999345678" },
                    { 4, "Ramos Espinar", "test4@test.com", "Ana", "999456789" },
                    { 5, "Escámez García", "test5@test.com", "Laura", "999987654" },
                    { 6, "González", "test6@test.com", "Sofía", "3468683" },
                    { 7, "Rodríguez", "test7@test.com", "Alejandro", "79934563" },
                    { 8, "García", "test8@test.com", "Laura", "" },
                    { 9, "López", "test9@test.com", "David", "82663902" },
                    { 10, "Martín", "test10@test.com", "María", "" }
                });

            migrationBuilder.InsertData(
                table: "Libros",
                columns: new[] { "Id", "AutorId", "CategoriaId", "ClienteId", "FechaPublicacion", "ISBN", "Imagen", "Titulo" },
                values: new object[,]
                {
                    { 1, 1, 1, null, new DateTime(2024, 9, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "087402", null, "Los Pilares de la Tierra" },
                    { 2, 2, 2, null, new DateTime(2024, 9, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "4684", null, "Moby Dick" },
                    { 3, 3, 3, null, new DateTime(2024, 9, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "68453", null, "El resplandor" },
                    { 4, 4, 4, null, new DateTime(2024, 9, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "867832", null, "La metamorfosis" },
                    { 5, 5, 5, null, new DateTime(2024, 9, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "0275935", null, "En busca del tiempo perdido" },
                    { 6, 6, 1, null, new DateTime(2024, 9, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "0275935", null, "El Retrato de Dorian Gray" },
                    { 7, 7, 2, null, new DateTime(2024, 9, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "0275935", null, "La vuelta al mundo en 80 días" },
                    { 8, 8, 3, null, new DateTime(2024, 9, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "0275935", null, "Guerra y Paz" },
                    { 9, 9, 4, null, new DateTime(2024, 9, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "0275935", null, "Frankenstein" },
                    { 10, 10, 5, null, new DateTime(2024, 9, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "0275935", null, "Los miserables" },
                    { 11, 11, 1, null, new DateTime(2024, 9, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "0275935", null, "El talento de Mr. Ripley" },
                    { 12, 12, 2, null, new DateTime(2024, 9, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "0275935", null, "Romeo y Julieta" },
                    { 13, 13, 3, null, new DateTime(2024, 9, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "0275935", null, "La casa de Bernarda Alba" },
                    { 14, 14, 4, null, new DateTime(2024, 9, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "0275935", null, "Oliver Twist" },
                    { 15, 15, 5, null, new DateTime(2024, 9, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "0275935", null, "El alquimista" }
                });

            migrationBuilder.InsertData(
                table: "Prestamo",
                columns: new[] { "Id", "ClienteId", "FechaDevolucion", "FechaPrestamo", "LibroId" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2024, 9, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 9, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 2, 2, new DateTime(2024, 9, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 9, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 2 },
                    { 3, 3, new DateTime(2024, 9, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 9, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 3 },
                    { 4, 4, new DateTime(2024, 9, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 9, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 4 },
                    { 5, 5, new DateTime(2024, 9, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 9, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 5 },
                    { 6, 6, new DateTime(2024, 9, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 9, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 6 },
                    { 7, 7, new DateTime(2024, 9, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 9, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 7 },
                    { 8, 8, new DateTime(2024, 9, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 9, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 8 },
                    { 9, 9, new DateTime(2024, 9, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 9, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 9 },
                    { 10, 10, new DateTime(2024, 9, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 9, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 10 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Libros_AutorId",
                table: "Libros",
                column: "AutorId");

            migrationBuilder.CreateIndex(
                name: "IX_Libros_CategoriaId",
                table: "Libros",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Libros_ClienteId",
                table: "Libros",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Prestamo_ClienteId",
                table: "Prestamo",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Prestamo_LibroId",
                table: "Prestamo",
                column: "LibroId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Prestamo");

            migrationBuilder.DropTable(
                name: "Libros");

            migrationBuilder.DropTable(
                name: "Autores");

            migrationBuilder.DropTable(
                name: "Categorias");

            migrationBuilder.DropTable(
                name: "Clientes");
        }
    }
}
