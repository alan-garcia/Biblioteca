using AutoMapper;
using BibliotecaNET8.Models;
using BibliotecaNET8.ViewModels.Autor;
using BibliotecaNET8.ViewModels.Categoria;
using BibliotecaNET8.ViewModels.Cliente;
using BibliotecaNET8.ViewModels.Libro;
using BibliotecaNET8.ViewModels.Prestamo;

namespace BibliotecaNET8.Mappers;

public class AutoMapping : Profile
{
    public AutoMapping()
    {
        CreateMap<Autor, AutorVM>().ReverseMap();
        CreateMap<Categoria, CategoriaVM>().ReverseMap();
        CreateMap<Cliente, ClienteVM>().ReverseMap();

        CreateMap<Libro, LibroCreateVM>()
            .ReverseMap()
            .ForMember(dest => dest.Imagen, opt => opt.Condition(src => src.Imagen != null));

        CreateMap<Prestamo, PrestamoCreateVM>().ReverseMap();
    }
}
