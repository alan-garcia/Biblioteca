using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using BibliotecaNET8.Models;
using BibliotecaNET8.Services;
using BibliotecaNET8.Utils;
using BibliotecaNET8.ViewModels;
using BibliotecaNET8.ViewModels.Libro;

namespace BibliotecaNET8.Controllers;

public class LibroController : Controller
{
    private readonly ILibroService _libroService;
    private readonly IAutorService _autorService;
    private readonly ICategoriaService _categoriaService;
    private readonly IStringLocalizer<Translations> _localizer;
    private readonly IMapper _mapper;
    private readonly IValidator<LibroCreateVM> _libroValidator;

    public LibroController(ILibroService libroService, IAutorService autorService, ICategoriaService categoriaService,
        IStringLocalizer<Translations> localizer, IMapper mapper, IValidator<LibroCreateVM> libroValidator)
    {
        _libroService = libroService;
        _autorService = autorService;
        _categoriaService = categoriaService;
        _localizer = localizer;
        _mapper = mapper;
        _libroValidator = libroValidator;
    }

    public async Task<IActionResult> Index(string? term = "", int pageNumber = PaginationSettings.PageNumber,
        int pageSize = PaginationSettings.PageSize)
    {
        term = term?.ToLower().Trim();
        ViewData["CurrentSearchTerm"] = term;

        PagedResult<LibroListVM> librosPagedVM = new();
        IQueryable<Libro> libros = _libroService.GetLibrosConAutoresCategorias();
        if (!string.IsNullOrEmpty(term))
        {
            libros = _libroService.SearchLibro(libros, libro =>
                libro.Titulo.ToLower().Contains(term) ||
                libro.ISBN.ToLower().Contains(term));
        }

        if (!libros.Any())
        {
            ViewBag.ListaLibros = _localizer["ListaLibrosEmptyMessage"];
        }
        else
        {
            PagedResult<Libro> pagedResult = await _libroService.GetRecordsPagedResult(libros, pageNumber, pageSize);

            librosPagedVM = new PagedResult<LibroListVM>
            {
                PageNumber = pagedResult.PageNumber,
                PageSize = pagedResult.PageSize,
                TotalItems = pagedResult.TotalItems,
                TotalPages = (int)Math.Ceiling(pagedResult.TotalItems / (double)pageSize),
                Items = pagedResult.Items.Select(libro => new LibroListVM
                {
                    Titulo = libro.Titulo,
                    FechaPublicacion = libro.FechaPublicacion,
                    ISBN = libro.ISBN,
                    Autor = new Autor
                    {
                        Id = libro.Autor.Id,
                        Nombre = libro.Autor.Nombre,
                        Apellido = libro.Autor.Apellido,
                        FechaNacimiento = libro.Autor.FechaNacimiento
                    },
                    Categoria = libro.Categoria,
                    Id = libro.Id
                }).ToList()
            };
        }

        return View(librosPagedVM);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        LibroCreateVM? libroVM = await LoadAutoresCategoriasDropdownList();
        if (!libroVM.Autores.Any() || !libroVM.Categorias.Any())
        {
            TempData["ExisteAutorCategoria"] = _localizer["ExisteAutorCategoriaCreate"].Value;
            return RedirectToAction("Index");
        }

        return View(libroVM);
    }

    [HttpPost]
    public async Task<IActionResult> Create(IFormFile? Imagen, LibroCreateVM libroVM)
    {
        ValidationResult result = await _libroValidator.ValidateAsync(libroVM);
        if (result.IsValid)
        {
            try
            {
                Libro? libro = _mapper.Map<LibroCreateVM, Libro>(libroVM);
                var (libroFinal, errorMensaje) = await _libroService.SetBinaryImage(Imagen, libro);
                if (errorMensaje != null)
                {
                    ModelState.AddModelError("Imagen", errorMensaje);
                    libroVM = await LoadAutoresCategoriasDropdownList();
                    return View(libroVM);
                }

                await _libroService.AddLibro(libroFinal);
                TempData["LibroMensajes"] = _localizer["LibroCreadoMessageSuccess"].Value;

                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                return View("404");
            }
        }

        result.AddToModelState(ModelState);
        libroVM = await LoadAutoresCategoriasDropdownList();

        return View(libroVM);
    }

    [HttpGet]
    public async Task<IActionResult> Ver(int? id)
    {
        try
        {
            Libro? libro = await _libroService.GetLibroById(id);

            LibroCreateVM libroVM = await LoadAutoresCategoriasDropdownList();
            libroVM.Id = libro.Id;
            libroVM.Titulo = libro.Titulo;
            libroVM.ISBN = libro.ISBN;
            libroVM.FechaPublicacion = libro.FechaPublicacion;
            libroVM.AutorId = libro.AutorId;
            libroVM.CategoriaId = libro.CategoriaId;
            libroVM.Imagen = libro.Imagen;

            return View(libroVM);
        }
        catch (Exception)
        {
            return View("404");
        }
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int? id)
    {
        LibroCreateVM libroVM;
        try
        {
            Libro? libro = await _libroService.GetLibroById(id);

            libroVM = await LoadAutoresCategoriasDropdownList();
            libroVM.Id = libro.Id;
            libroVM.Titulo = libro.Titulo;
            libroVM.ISBN = libro.ISBN;
            libroVM.FechaPublicacion = libro.FechaPublicacion;
            libroVM.AutorId = libro.AutorId;
            libroVM.CategoriaId = libro.CategoriaId;
            libroVM.Imagen = libro.Imagen;

            if (!libroVM.Autores.Any() || !libroVM.Categorias.Any())
            {
                TempData["ExisteAutorCategoria"] = _localizer["ExisteAutorCategoriaEdit"].Value;
                return RedirectToAction("Index");
            }
        }
        catch (Exception)
        {
            return View("404");
        }

        return View(libroVM);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(IFormFile? Imagen, string? ImagenActual, LibroCreateVM libroVM)
    {
        ValidationResult result = await _libroValidator.ValidateAsync(libroVM);
        if (result.IsValid)
        {
            try
            {
                Libro? libro = _mapper.Map<LibroCreateVM, Libro>(libroVM);
                var (libroFinal, errorMensaje) = await _libroService.SetBinaryImage(Imagen, libro, ImagenActual);
                if (errorMensaje != null)
                {
                    ModelState.AddModelError("Imagen", errorMensaje);
                    libroVM = await LoadAutoresCategoriasDropdownList();
                    if (!string.IsNullOrEmpty(ImagenActual))
                    {
                        libroVM.Imagen = Convert.FromBase64String(ImagenActual);
                    }
                    return View(libroVM);
                }

                await _libroService.UpdateLibro(libroFinal);
                TempData["LibroMensajes"] = _localizer["LibroModificadoMessageSuccess"].Value;

                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                return View("404");
            }
        }
        else
        {
            libroVM = await LoadAutoresCategoriasDropdownList();
        }

        result.AddToModelState(ModelState);

        return View(libroVM);
    }

    [HttpPost]
    public async Task<JsonResult> Delete(int? id)
    {
        bool isDeleted;
        try
        {
            isDeleted = await _libroService.DeleteLibro(id);
            if (isDeleted)
            {
                TempData["LibroMensajes"] = _localizer["LibroEliminadoMessageSuccess"].Value;
            }
            else
            {
                TempData["LibroMensajes"] = _localizer["LibroEliminadoMessageFail"].Value;
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al eliminar el libro: {ex.Message}");
        }
        
        return Json(new
        {
            success = isDeleted,
            mensaje = TempData["LibroMensajes"]
        });
    }

    [HttpPost]
    public async Task<JsonResult> DeleteMultiple([FromBody] int[] idsLibro)
    {
        bool isDeleted;
        try
        {
            isDeleted = await _libroService.DeleteMultipleLibros(idsLibro);
            if (isDeleted)
            {
                TempData["LibroMensajes"] = _localizer["LibroEliminadoMultipleMessageSuccess"].Value;
            }
            else
            {
                TempData["LibroMensajes"] = _localizer["LibroEliminadoMultipleMessageFail"].Value;
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al eliminar múltiples clientes: {ex.Message}");
        }

        return Json(new
        {
            success = isDeleted,
            mensaje = TempData["LibroMensajes"]
        });
    }

    public async Task<IActionResult> Search(string? term = "", int pageNumber = PaginationSettings.PageNumber,
        int pageSize = PaginationSettings.PageSize)
    {
        term = (term == PaginationSettings.ShowAllRecordsText) ? string.Empty : term?.ToLower().Trim();
        ViewData["CurrentSearchTerm"] = term;
        IQueryable<Libro> filtroLibros;
        IQueryable<Libro> filtroLibrosSearch;
        try
        {
            filtroLibros = _libroService.GetLibrosConAutoresCategorias();
            filtroLibrosSearch = _libroService.SearchLibro(filtroLibros, libro =>
                libro.Titulo.ToLower().Contains(term) ||
                libro.ISBN.ToLower().Contains(term));
        }
        catch (Exception)
        {
            throw new Exception("Error al buscar libros");
        }

        PagedResult<Libro> pagedResult = await _libroService.GetRecordsPagedResult(filtroLibrosSearch, pageNumber, pageSize);

        PagedResult<LibroListVM> librosPagedVM = new()
        {
            PageNumber = pagedResult.PageNumber,
            PageSize = pagedResult.PageSize,
            TotalItems = pagedResult.TotalItems,
            TotalPages = (int)Math.Ceiling(pagedResult.TotalItems / (double)pageSize),
            Items = pagedResult.Items.Select(libro => new LibroListVM
            {
                Id = libro.Id,
                Titulo = libro.Titulo,
                FechaPublicacion = libro.FechaPublicacion,
                ISBN = libro.ISBN,
                AutorId = libro.AutorId,
                CategoriaId = libro.CategoriaId,
                Autor = libro.Autor,
                Categoria = libro.Categoria,
            }).ToList()
        };

        return PartialView("_LibrosTabla", librosPagedVM);
    }

    private async Task<LibroCreateVM> LoadAutoresCategoriasDropdownList(LibroCreateVM? libroVM = null)
    {
        IEnumerable<Autor> autores = await _autorService.GetAllAutores();
        IEnumerable<Categoria> categorias = await _categoriaService.GetAllCategorias();

        libroVM ??= new LibroCreateVM();
        libroVM.Autores = autores.Select(autor => new SelectListItem
        {
            Value = autor.Id.ToString(),
            Text = $"{autor.Nombre} {autor.Apellido}",
            Selected = libroVM.AutorId != 0 && autor.Id == libroVM.AutorId
        })
        .ToList();

        libroVM.Categorias = categorias.Select(categoria => new SelectListItem
        {
            Value = categoria.Id.ToString(),
            Text = categoria.Nombre,
            Selected = libroVM.CategoriaId != 0 && categoria.Id == libroVM.CategoriaId
        })
        .ToList();

        return libroVM;
    }
}
