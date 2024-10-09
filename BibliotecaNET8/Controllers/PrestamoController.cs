using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using FluentValidation;
using FluentValidation.AspNetCore;
using BibliotecaNET8.Application.Services.Interfaces;
using BibliotecaNET8.Infrastructure.Utils;
using BibliotecaNET8.Domain.Entities;
using BibliotecaNET8.Domain;
using BibliotecaNET8.Web.ViewModels.Prestamo;
using BibliotecaNET8.Application.DTOs.Prestamo;
using BibliotecaNET8.Domain.UnitOfWork.Interfaces;

namespace BibliotecaNET8.Web.Controllers;

public class PrestamoController : Controller
{
    private readonly IPrestamoService _prestamoService;
    private readonly ILibroService _libroService;
    private readonly IClienteService _clienteService;
    private readonly IStringLocalizer<Translations> _localizer;
    private readonly IMapper _mapper;
    private readonly IValidator<PrestamoCreateDTO> _prestamoValidator;
    private readonly IUnitOfWork _unitOfWork;

    public PrestamoController(IPrestamoService prestamoService, ILibroService libroService,
        IClienteService clienteRepository, IStringLocalizer<Translations> localizer, IMapper mapper,
        IValidator<PrestamoCreateDTO> prestamoValidator, IUnitOfWork unitOfWork)
    {
        _prestamoService = prestamoService;
        _libroService = libroService;
        _clienteService = clienteRepository;
        _localizer = localizer;
        _mapper = mapper;
        _prestamoValidator = prestamoValidator;
        _unitOfWork = unitOfWork;
    }

    public async Task<IActionResult> Index(string? term = "", int pageNumber = PaginationSettings.PageNumber,
        int pageSize = PaginationSettings.PageSize)
    {
        IQueryable<Prestamo> prestamos = _prestamoService.GetPrestamosConLibrosClientes();
        PagedResult<PrestamoListVM> prestamosPagedVM = new();

        term = term?.ToLower().Trim();
        ViewData["CurrentSearchTerm"] = term;
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
            prestamosPagedVM = _mapper.Map<PagedResult<PrestamoListVM>>(source: pagedResult);
        }

        return View(prestamosPagedVM);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        PrestamoCreateVM prestamoVM = await LoadClientesLibrosDropdownList();
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
        PrestamoCreateDTO prestamoDTO = _mapper.Map<PrestamoCreateDTO>(source: prestamoVM);
        var result = await _prestamoValidator.ValidateAsync(prestamoDTO);
        if (result.IsValid)
        {
            try
            {
                Prestamo prestamo = _mapper.Map<Prestamo>(source: prestamoDTO);
                await _prestamoService.AddPrestamo(prestamo);
                await _unitOfWork.Save();
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
            Prestamo prestamo = await _prestamoService.GetPrestamoById(id);

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
            Prestamo prestamo = await _prestamoService.GetPrestamoById(id);

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
        PrestamoCreateDTO prestamoDTO = _mapper.Map<PrestamoCreateDTO>(source: prestamoVM);
        var result = await _prestamoValidator.ValidateAsync(prestamoDTO);
        if (result.IsValid)
        {
            Prestamo? prestamo = _mapper.Map<Prestamo>(source: prestamoDTO);
            await _prestamoService.UpdatePrestamo(prestamo);
            await _unitOfWork.Save();
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
        string message;
        try
        {
            isDeleted = await _prestamoService.DeletePrestamo(id);
            if (isDeleted)
            {
                message = _localizer["PrestamoEliminadoMessageSuccess"].Value;
            }
            else
            {
                message = _localizer["PrestamoEliminadoMessageFail"].Value;
            }

            await _unitOfWork.Save();
            TempData["PrestamoMensajes"] = message;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al eliminar el préstamo: {ex.Message}");
        }

        return Json(new
        {
            success = isDeleted,
            mensaje = message
        });
    }

    [HttpPost]
    public async Task<JsonResult> DeleteMultiple([FromBody] int[] idsPrestamo)
    {
        bool isDeleted;
        string message;
        try
        {
            isDeleted = await _prestamoService.DeleteMultiplePrestamos(idsPrestamo);
            if (isDeleted)
            {
                message = _localizer["PrestamoEliminadoMultipleMessageSuccess"].Value;
            }
            else
            {
                message = _localizer["PrestamoEliminadoMultipleMessageFail"].Value;
            }

            await _unitOfWork.Save();
            TempData["ClientesMensaje"] = message;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al eliminar múltiples préstamos: {ex.Message}");
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
        PagedResult<PrestamoListVM> prestamosPagedVM = _mapper.Map<PagedResult<PrestamoListVM>>(source: pagedResult);

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
