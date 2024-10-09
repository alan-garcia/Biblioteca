using AutoMapper;
using BibliotecaNET8.Application.DTOs.Autor;
using BibliotecaNET8.Application.DTOs.Categoria;
using BibliotecaNET8.Application.DTOs.Cliente;
using BibliotecaNET8.Application.DTOs.Libro;
using BibliotecaNET8.Application.DTOs.Prestamo;
using BibliotecaNET8.Domain;
using BibliotecaNET8.Domain.Entities;

namespace BibliotecaNET8.Application.MappingProfiles;

/// <summary>
///     Configuración de AutoMapper cuyas clases de destino se utilizan en la capa de Application
/// </summary>
public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Autor, AutorDTO>().ReverseMap();
        CreateMap<Categoria, CategoriaDTO>().ReverseMap();
        CreateMap<Cliente, ClienteDTO>().ReverseMap();
        CreateMap<Libro, LibroListDTO>().ReverseMap();
        CreateMap<Libro, LibroCreateDTO>()
            .ReverseMap()
            .ForMember(libro => libro.Imagen, opt => opt.Condition(libroDto => libroDto.Imagen != null));
        CreateMap<Prestamo, PrestamoCreateDTO>().ReverseMap();
        CreateMap<Prestamo, PrestamoListDTO>().ReverseMap();

        CreateMap<PagedResult<Autor>, PagedResult<AutorDTO>>().ReverseMap();
        CreateMap<PagedResult<Categoria>, PagedResult<CategoriaDTO>>().ReverseMap();
        CreateMap<PagedResult<Cliente>, PagedResult<ClienteDTO>>().ReverseMap();
        CreateMap<PagedResult<Libro>, PagedResult<LibroListDTO>>().ReverseMap();
        CreateMap<PagedResult<Prestamo>, PagedResult<PrestamoListDTO>>().ReverseMap();
    }
}
