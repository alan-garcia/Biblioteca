using AutoMapper;
using BibliotecaNET8.Application.DTOs.Libro;
using BibliotecaNET8.Application.Services.Interfaces;
using BibliotecaNET8.Domain;
using BibliotecaNET8.Domain.Entities;
using BibliotecaNET8.Domain.UnitOfWork.Interfaces;
using BibliotecaNET8.Infrastructure.Utils;
using BibliotecaNET8.Web.ViewModels.Libro;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;

namespace BibliotecaNET8.Web.Controllers;

/// <summary>
///     Funcionalidades de la vista "Libro"
/// </summary>
public class LibroController : Controller
{
    private readonly ILibroService _libroService;
    private readonly IAutorService _autorService;
    private readonly ICategoriaService _categoriaService;
    private readonly IStringLocalizer<Translations> _localizer;
    private readonly IMapper _mapper;
    private readonly IValidator<LibroCreateDTO> _libroValidator;
    private readonly IUnitOfWork _unitOfWork;

    public LibroController(ILibroService libroService, IAutorService autorService, ICategoriaService categoriaService,
        IStringLocalizer<Translations> localizer, IMapper mapper, IValidator<LibroCreateDTO> libroValidator,
        IUnitOfWork unitOfWork)
    {
        _libroService = libroService;
        _autorService = autorService;
        _categoriaService = categoriaService;
        _localizer = localizer;
        _mapper = mapper;
        _libroValidator = libroValidator;
        _unitOfWork = unitOfWork;
    }

    public async Task<IActionResult> Index(string? term = "", int pageNumber = PaginationSettings.PageNumber,
        int pageSize = PaginationSettings.PageSize)
    {
        IQueryable<Libro> libros = _libroService.GetLibrosConAutoresCategorias();
        PagedResult<LibroListVM> librosPagedVM = new();
        
        term = term?.ToLower().Trim();
        ViewData["CurrentSearchTerm"] = term;
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
            librosPagedVM = _mapper.Map<PagedResult<LibroListVM>>(source: pagedResult);
        }

        return View(librosPagedVM);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        LibroCreateVM libroVM = await LoadAutoresCategoriasDropdownList();
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
        LibroCreateDTO libroDTO = _mapper.Map<LibroCreateDTO>(source: libroVM);
        var result = await _libroValidator.ValidateAsync(libroDTO);
        if (result.IsValid)
        {
            try
            {
                Libro libro = _mapper.Map<Libro>(source: libroDTO);
                var (libroFinal, errorMensaje) = await _libroService.SetBinaryImage(Imagen, libro);
                if (errorMensaje != null)
                {
                    ModelState.AddModelError("Imagen", errorMensaje);
                    libroVM = await LoadAutoresCategoriasDropdownList();
                    return View(libroVM);
                }

                await _libroService.AddLibro(libroFinal);
                await _unitOfWork.Save();
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
            Libro libro = await _libroService.GetLibroById(id);

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
            Libro libro = await _libroService.GetLibroById(id);

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
        LibroCreateDTO libroDTO = _mapper.Map<LibroCreateDTO>(source: libroVM);
        var result = await _libroValidator.ValidateAsync(libroDTO);
        if (result.IsValid)
        {
            try
            {
                Libro libro = _mapper.Map<Libro>(source: libroDTO);
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
                await _unitOfWork.Save();
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
        string message;
        try
        {
            isDeleted = await _libroService.DeleteLibro(id);
            if (isDeleted)
            {
                message = _localizer["LibroEliminadoMessageSuccess"].Value;
            }
            else
            {
                message = _localizer["LibroEliminadoMessageFail"].Value;
            }

            await _unitOfWork.Save();
            TempData["LibroMensajes"] = message;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al eliminar el libro: {ex.Message}");
        }
        
        return Json(new
        {
            success = isDeleted,
            mensaje = message
        });
    }

    [HttpPost]
    public async Task<JsonResult> DeleteMultiple([FromBody] int[] idsLibro)
    {
        bool isDeleted;
        string message;
        try
        {
            isDeleted = await _libroService.DeleteMultipleLibros(idsLibro);
            if (isDeleted)
            {
                message = _localizer["LibroEliminadoMultipleMessageSuccess"].Value;
            }
            else
            {
                message = _localizer["LibroEliminadoMultipleMessageFail"].Value;
            }

            await _unitOfWork.Save();
            TempData["LibroMensajes"] = message;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al eliminar múltiples clientes: {ex.Message}");
        }

        return Json(new
        {
            success = isDeleted,
            mensaje = message
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
        PagedResult<LibroListVM> librosPagedVM = _mapper.Map<PagedResult<LibroListVM>>(source: pagedResult);

        return PartialView("_LibrosTabla", librosPagedVM);
    }

    /// <summary>
    ///     Carga toda la lista de autores y categorías en el elemento <select></select> del formulario
    /// </summary>
    /// <param name="libroVM">El ViewModel a asignar en la vista.</param>
    /// <returns>ViewModel con la lista de autores y categorías cargadas</returns>
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
