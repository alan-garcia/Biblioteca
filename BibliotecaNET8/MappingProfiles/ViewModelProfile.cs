using AutoMapper;
using BibliotecaNET8.Application.DTOs.Autor;
using BibliotecaNET8.Application.DTOs.Categoria;
using BibliotecaNET8.Application.DTOs.Cliente;
using BibliotecaNET8.Application.DTOs.Libro;
using BibliotecaNET8.Application.DTOs.Prestamo;
using BibliotecaNET8.Domain;
using BibliotecaNET8.Domain.Entities;
using BibliotecaNET8.Web.ViewModels.Autor;
using BibliotecaNET8.Web.ViewModels.Categoria;
using BibliotecaNET8.Web.ViewModels.Cliente;
using BibliotecaNET8.Web.ViewModels.Libro;
using BibliotecaNET8.Web.ViewModels.Prestamo;

namespace BibliotecaNET8.Web.MappingProfiles;

/// <summary>
///     Configuración de AutoMapper cuyas clases de destino se utilizan en la capa de presentación
/// </summary>
public class ViewModelProfile : Profile
{
    public ViewModelProfile()
    {
        CreateMap<Autor, AutorVM>().ReverseMap();
        CreateMap<Categoria, CategoriaVM>().ReverseMap();
        CreateMap<Cliente, ClienteVM>().ReverseMap();
        CreateMap<Libro, LibroListVM>().ReverseMap();
        CreateMap<Libro, LibroCreateVM>()
            .ReverseMap()
            .ForMember(libro => libro.Imagen, opt => opt.Condition(libroVM => libroVM.Imagen != null));
        CreateMap<Prestamo, PrestamoCreateVM>().ReverseMap();
        CreateMap<Prestamo, PrestamoListVM>().ReverseMap();

        CreateMap<AutorDTO, AutorVM>().ReverseMap();
        CreateMap<CategoriaDTO, CategoriaVM>().ReverseMap();
        CreateMap<ClienteDTO, ClienteVM>().ReverseMap();
        CreateMap<LibroListDTO, LibroListVM>().ReverseMap();
        CreateMap<LibroCreateDTO, LibroCreateVM>()
            .ReverseMap()
            .ForMember(libroDto => libroDto.Imagen, opt => opt.Condition(libroVM => libroVM.Imagen != null));
        CreateMap<PrestamoCreateDTO, PrestamoCreateVM>().ReverseMap();
        CreateMap<PrestamoListDTO, PrestamoListVM>().ReverseMap();

        CreateMap<PagedResult<Autor>, PagedResult<AutorVM>>().ReverseMap();
        CreateMap<PagedResult<Categoria>, PagedResult<CategoriaVM>>().ReverseMap();
        CreateMap<PagedResult<Cliente>, PagedResult<ClienteVM>>().ReverseMap();
        CreateMap<PagedResult<Libro>, PagedResult<LibroListVM>>().ReverseMap();
        CreateMap<PagedResult<Prestamo>, PagedResult<PrestamoListVM>>().ReverseMap();
    }
}
