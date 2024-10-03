using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using BibliotecaNET8.Models;
using BibliotecaNET8.Services;
using BibliotecaNET8.Utils;
using BibliotecaNET8.ViewModels;
using BibliotecaNET8.ViewModels.Prestamo;
using BibliotecaNET8.ViewModels.Libro;
using FluentValidation;
using FluentValidation.Results;
using FluentValidation.AspNetCore;

namespace BibliotecaNET8.Controllers;

public class PrestamoController : Controller
{
    private readonly IPrestamoService _prestamoService;
    private readonly ILibroService _libroService;
    private readonly IClienteService _clienteService;
    private readonly IStringLocalizer<Translations> _localizer;
    private readonly IMapper _mapper;
    private readonly IValidator<PrestamoCreateVM> _prestamoValidator;

    public PrestamoController(IPrestamoService prestamoService, ILibroService libroService,
        IClienteService clienteRepository, IStringLocalizer<Translations> localizer, IMapper mapper,
        IValidator<PrestamoCreateVM> prestamoValidator)
    {
        _prestamoService = prestamoService;
        _libroService = libroService;
        _clienteService = clienteRepository;
        _localizer = localizer;
        _mapper = mapper;
        _prestamoValidator = prestamoValidator;
    }

    public async Task<IActionResult> Index(string? term = "", int pageNumber = PaginationSettings.PageNumber,
        int pageSize = PaginationSettings.PageSize)
    {
        term = term?.ToLower().Trim();
        ViewData["CurrentSearchTerm"] = term;

        PagedResult<PrestamoListVM> prestamosVM = new();
        IQueryable<Prestamo> prestamos = _prestamoService.GetPrestamosConLibrosClientes();
        if (!string.IsNullOrEmpty(term))
        {
            prestamos = _prestamoService.SearchPrestamo(prestamos, prestamo =>
                prestamo.Cliente.Nombre.ToLower().Contains(term) ||
                prestamo.Cliente.Nombre.ToLower().Contains(term) ||
                prestamo.Libro.Titulo.ToLower().Contains(term));
        }

        if (!prestamos.Any())
        {
            ViewBag.ListaPrestamos = _localizer["ListaPrestamosEmptyMessage"];
        }
        else
        {
            PagedResult<Prestamo> pagedResult = await _prestamoService.GetRecordsPagedResult(prestamos, pageNumber, pageSize);

            prestamosVM = new PagedResult<PrestamoListVM>
            {
                PageNumber = pagedResult.PageNumber,
                PageSize = pagedResult.PageSize,
                TotalItems = pagedResult.TotalItems,
                TotalPages = (int)Math.Ceiling(pagedResult.TotalItems / (double)pageSize),
                Items = pagedResult.Items.Select(prestamo => new PrestamoListVM
                {
                    FechaPrestamo = prestamo.FechaPrestamo,
                    FechaDevolucion = prestamo.FechaDevolucion,
                    Cliente = new Cliente
                    {
                        Id = prestamo.Cliente.Id,
                        Nombre = prestamo.Cliente.Nombre,
                        Apellido = prestamo.Cliente.Apellido,
                        Email = prestamo.Cliente.Email,
                        Telefono = prestamo.Cliente.Telefono
                    },
                    Libro = new Libro
                    {
                        Id = prestamo.Libro.Id,
                        Titulo = prestamo.Libro.Titulo,
                        ISBN = prestamo.Libro.ISBN,
                        FechaPublicacion = prestamo.Libro.FechaPublicacion
                    },
                    Id = prestamo.Id,
                    ClienteId = prestamo.ClienteId,
                    LibroId = prestamo.LibroId
                }).ToList()
            };
        }

        return View(prestamosVM);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        PrestamoCreateVM? prestamoVM = await LoadClientesLibrosDropdownList();
        if (!prestamoVM.Libros.Any() || !prestamoVM.Clientes.Any())
        {
            TempData["ExisteLibroClienteCreate"] = _localizer["ExisteLibroClienteCreate"].Value;
            return RedirectToAction("Index");
        }

        return View(prestamoVM);
    }

    [HttpPost]
    public async Task<IActionResult> Create(PrestamoCreateVM prestamoVM)
    {
        ValidationResult result = await _prestamoValidator.ValidateAsync(prestamoVM);
        if (result.IsValid)
        {
            try
            {
                Prestamo? prestamo = _mapper.Map<PrestamoCreateVM, Prestamo>(prestamoVM);
                await _prestamoService.AddPrestamo(prestamo);
                TempData["PrestamoMensajes"] = _localizer["PrestamoCreadoMessageSuccess"].Value;

                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                return View("404");
            }
        }

        result.AddToModelState(ModelState);
        prestamoVM = await LoadClientesLibrosDropdownList();

        return View(prestamoVM);
    }

    [HttpGet]
    public async Task<IActionResult> Ver(int? id)
    {
        try
        {
            Prestamo? prestamo = await _prestamoService.GetPrestamoById(id);

            PrestamoCreateVM prestamoVM = await LoadClientesLibrosDropdownList();
            prestamoVM.Id = prestamo.Id;
            prestamoVM.FechaPrestamo = prestamo.FechaPrestamo;
            prestamoVM.FechaDevolucion = prestamo.FechaDevolucion;
            prestamoVM.LibroId = prestamo.LibroId;
            prestamoVM.ClienteId = prestamo.ClienteId;

            return View(prestamoVM);
        }
        catch (Exception)
        {
            return View("404");
        }
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int? id)
    {
        PrestamoCreateVM prestamoVM;
        try
        {
            Prestamo? prestamo = await _prestamoService.GetPrestamoById(id);

            prestamoVM = await LoadClientesLibrosDropdownList();
            prestamoVM.Id = prestamo.Id;
            prestamoVM.FechaPrestamo = prestamo.FechaPrestamo;
            prestamoVM.FechaDevolucion = prestamo.FechaDevolucion;
            prestamoVM.LibroId = prestamo.LibroId;
            prestamoVM.ClienteId = prestamo.ClienteId;

            if (!prestamoVM.Libros.Any() || !prestamoVM.Clientes.Any())
            {
                TempData["ExisteLibroClienteEdit"] = _localizer["ExisteLibroClienteEdit"].Value;
                return RedirectToAction("Index");
            }
        }
        catch (Exception)
        {
            return View("404");
        }

        return View(prestamoVM);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(PrestamoCreateVM prestamoVM)
    {
        ValidationResult result = await _prestamoValidator.ValidateAsync(prestamoVM);
        if (result.IsValid)
        {
            Prestamo? prestamo = _mapper.Map<PrestamoCreateVM, Prestamo>(prestamoVM);
            await _prestamoService.UpdatePrestamo(prestamo);
            TempData["PrestamoMensajes"] = _localizer["PrestamoModificadoMessageSuccess"].Value;

            return RedirectToAction(nameof(Index));
        }

        result.AddToModelState(ModelState);

        return View(prestamoVM);
    }

    [HttpPost]
    public async Task<JsonResult> Delete(int? id)
    {
        bool isDeleted;
        try
        {
            isDeleted = await _prestamoService.DeletePrestamo(id);
            if (isDeleted)
            {
                TempData["PrestamoMensajes"] = _localizer["PrestamoEliminadoMessageSuccess"].Value;
            }
            else
            {
                TempData["PrestamoMensajes"] = _localizer["PrestamoEliminadoMessageFail"].Value;
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al eliminar el préstamo: {ex.Message}");
        }

        return Json(new
        {
            success = isDeleted,
            mensaje = TempData["PrestamoMensajes"]
        });
    }

    [HttpPost]
    public async Task<JsonResult> DeleteMultiple([FromBody] int[] idsPrestamo)
    {
        bool isDeleted;
        try
        {
            isDeleted = await _prestamoService.DeleteMultiplePrestamos(idsPrestamo);
            if (isDeleted)
            {
                TempData["PrestamoMensajes"] = _localizer["PrestamoEliminadoMultipleMessageSuccess"].Value;
            }
            else
            {
                TempData["PrestamoMensajes"] = _localizer["PrestamoEliminadoMultipleMessageFail"].Value;
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al eliminar múltiples préstamos: {ex.Message}");
        }

        return Json(new
        {
            success = isDeleted,
            mensaje = TempData["PrestamoMensajes"]
        });
    }

    public async Task<IActionResult> Search(string? term = "", int pageNumber = PaginationSettings.PageNumber,
        int pageSize = PaginationSettings.PageSize)
    {
        term = (term == PaginationSettings.ShowAllRecordsText) ? string.Empty : term?.ToLower().Trim();
        ViewData["CurrentSearchTerm"] = term;
        IQueryable<Prestamo> filtroPrestamos;
        IQueryable<Prestamo> filtroPrestamosSearch;
        try
        {
            filtroPrestamos = _prestamoService.GetPrestamosConLibrosClientes();
            filtroPrestamosSearch = _prestamoService.SearchPrestamo(filtroPrestamos, prestamo =>
                prestamo.Cliente.Nombre.ToLower().Contains(term) ||
                prestamo.Cliente.Nombre.ToLower().Contains(term) ||
                prestamo.Libro.Titulo.ToLower().Contains(term));
        }
        catch (Exception)
        {
            throw new Exception("Error al buscar préstamos");
        }

        PagedResult<Prestamo> pagedResult = await _prestamoService.GetRecordsPagedResult(filtroPrestamosSearch, pageNumber, pageSize);

        PagedResult<PrestamoListVM> prestamosPagedVM = new()
        {
            PageNumber = pagedResult.PageNumber,
            PageSize = pagedResult.PageSize,
            TotalItems = pagedResult.TotalItems,
            TotalPages = (int)Math.Ceiling(pagedResult.TotalItems / (double)pageSize),
            Items = pagedResult.Items.Select(prestamo => new PrestamoListVM
            {
                FechaPrestamo = prestamo.FechaPrestamo,
                FechaDevolucion = prestamo.FechaDevolucion,
                ClienteId = prestamo.ClienteId,
                LibroId = prestamo.LibroId,
                Cliente = prestamo.Cliente,
                Libro = prestamo.Libro
            }).ToList()
        };

        return PartialView("_PrestamosTabla", prestamosPagedVM);
    }

    private async Task<PrestamoCreateVM> LoadClientesLibrosDropdownList(PrestamoCreateVM? prestamoVM = null)
    {
        IEnumerable<Cliente> clientes = await _clienteService.GetAllClientes();
        IEnumerable<Libro> libros = await _libroService.GetAllLibros();

        prestamoVM ??= new PrestamoCreateVM();
        prestamoVM.Clientes = clientes.Select(cliente => new SelectListItem
        {
            Value = cliente.Id.ToString(),
            Text = $"{cliente.Nombre} {cliente.Apellido}",
            Selected = prestamoVM.Id != 0 && cliente.Id == prestamoVM.ClienteId
        })
        .ToList();

        prestamoVM.Libros = libros.Select(libro => new SelectListItem
        {
            Value = libro.Id.ToString(),
            Text = $"{libro.Titulo} - {libro.ISBN}",
            Selected = prestamoVM.LibroId != 0 && libro.Id == prestamoVM.LibroId
        })
        .ToList();

        return prestamoVM;
    }
}
