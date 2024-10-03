﻿using System.ComponentModel.DataAnnotations;

namespace BibliotecaNET8.Models;

public class Autor : BaseEntity
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Nombre { get; set; }

    [StringLength(100)]
    public string? Apellido { get; set; }

    [DataType(DataType.Date)]
    public DateTime FechaNacimiento { get; set; }

    public virtual ICollection<Libro> Libros { get; set; }
    
}
