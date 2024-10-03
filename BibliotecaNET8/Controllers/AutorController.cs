using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using BibliotecaNET8.Models;
using BibliotecaNET8.Services;
using BibliotecaNET8.Utils;
using BibliotecaNET8.ViewModels;
using BibliotecaNET8.ViewModels.Autor;

namespace BibliotecaNET8.Controllers;

public class AutorController : Controller
{
    private readonly IAutorService _autorService;
    private readonly IStringLocalizer<Translations> _localizer;
    private readonly IMapper _mapper;
    private readonly IValidator<AutorVM> _autorValidator;

    public AutorController(IAutorService autorService,IStringLocalizer<Translations> localizer, IMapper mapper,
        IValidator<AutorVM> autorValidator)
    {
        _autorService = autorService;
        _localizer = localizer;
        _mapper = mapper;
        _autorValidator = autorValidator;
    }

    public async Task<IActionResult> Index(string? term = "", int pageNumber = PaginationSettings.PageNumber,
        int pageSize = PaginationSettings.PageSize)
    {
        term = term?.ToLower().Trim();
        ViewData["CurrentSearchTerm"] = term;

        PagedResult<AutorVM> autoresPagedVM = new();
        IQueryable<Autor> autores = await _autorService.GetAllAutores();
        if (!string.IsNullOrEmpty(term))
        {
            autores = _autorService.SearchAutor(autores, autor =>
                autor.Nombre.ToLower().Contains(term) ||
                autor.Apellido.ToLower().Contains(term));
        }

        if (!autores.Any())
        {
            ViewBag.ListaAutores = _localizer["ListaAutoresEmptyMessage"];
        }
        else
        {
            PagedResult<Autor> pagedResult = await _autorService.GetRecordsPagedResult(autores, pageNumber, pageSize);

            autoresPagedVM = new PagedResult<AutorVM>
            {
                PageNumber = pagedResult.PageNumber,
                PageSize = pagedResult.PageSize,
                TotalItems = pagedResult.TotalItems,
                TotalPages = (int)Math.Ceiling(pagedResult.TotalItems / (double)pagedResult.PageSize),
                Items = pagedResult.Items.Select(autor => new AutorVM
                {
                    Id = autor.Id,
                    Nombre = autor.Nombre,
                    Apellido = autor.Apellido,
                    FechaNacimiento = autor.FechaNacimiento
                }).ToList()
            };
        }

        return View(autoresPagedVM);
    }

    [HttpGet]
    public IActionResult Create() => View();

    [HttpPost]
    public async Task<IActionResult> Create(AutorVM autorVM)
    {
        ValidationResult result = await _autorValidator.ValidateAsync(autorVM);
        if (result.IsValid)
        {
            try
            {
                Autor autor = _mapper.Map<AutorVM, Autor>(autorVM);
                await _autorService.AddAutor(autor);
                TempData["AutoresMensaje"] = _localizer["AutorCreadoMessageSuccess"].Value;

                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                return View("404");
            }
        }

        result.AddToModelState(ModelState);

        return View(autorVM);
    }

    [HttpGet]
    public async Task<IActionResult> Ver(int? id)
    {
        try
        {
            Autor autor = await _autorService.GetAutorById(id);
            AutorVM autorVM = _mapper.Map<Autor, AutorVM>(autor);

            return View(autorVM);
        }
        catch (Exception)
        {
            return View("404");
        }
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int? id)
    {
        try
        {
            Autor autor = await _autorService.GetAutorById(id);
            AutorVM autorVM = _mapper.Map<Autor, AutorVM>(autor);

            return View(autorVM);
        }
        catch (Exception)
        {
            return View("404");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Edit(AutorVM autorVM)
    {
        ValidationResult result = await _autorValidator.ValidateAsync(autorVM);
        if (result.IsValid)
        {
            Autor autor = _mapper.Map<AutorVM, Autor>(autorVM);
            await _autorService.UpdateAutor(autor);
            TempData["AutoresMensaje"] = _localizer["AutorModificadoMessageSuccess"].Value;

            return RedirectToAction(nameof(Index));
        }

        result.AddToModelState(ModelState);

        return View(autorVM);
    }

    [HttpPost]
    public async Task<JsonResult> Delete(int? id)
    {
        bool isDeleted;
        try
        {
            isDeleted = await _autorService.DeleteAutor(id);
            if (isDeleted)
            {
                TempData["AutoresMensajes"] = _localizer["AutorEliminadoMessageSuccess"].Value;
            }
            else
            {
                TempData["AutoresMensajes"] = _localizer["AutorEliminadoMessageFail"].Value;
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al eliminar el autor: {ex.Message}");
        }

        return Json(new
        {
            success = isDeleted,
            mensaje = TempData["AutoresMensajes"]
        });
    }

    [HttpPost]
    public async Task<JsonResult> DeleteMultiple([FromBody] int[] idsAutor)
    {
        bool isDeleted;
        try
        {
            isDeleted = await _autorService.DeleteMultipleAutores(idsAutor);
            if (isDeleted)
            {
                TempData["AutoresMensajes"] = _localizer["AutorEliminadoMultipleMessageSuccess"].Value;
            }
            else
            {
                TempData["AutoresMensajes"] = _localizer["AutorEliminadoMultipleMessageFail"].Value;
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al eliminar múltiples autores: {ex.Message}");
        }

        return Json(new
        {
            success = isDeleted,
            mensaje = TempData["AutoresMensajes"]
        });
    }

    public async Task<IActionResult> Search(string? term = "", int pageNumber = PaginationSettings.PageNumber,
        int pageSize = PaginationSettings.PageSize)
    {
        term = (term == PaginationSettings.ShowAllRecordsText) ? string.Empty : term?.ToLower().Trim();
        ViewData["CurrentSearchTerm"] = term;
        IQueryable<Autor> filtroAutores;
        try
        {
            filtroAutores = _autorService.SearchAutor(autor =>
                autor.Nombre.ToLower().Contains(term) ||
                autor.Apellido.ToLower().Contains(term));
        }
        catch (Exception)
        {
            throw new Exception("Error al buscar autores");
        }

        PagedResult<Autor> pagedResult = await _autorService.GetRecordsPagedResult(filtroAutores, pageNumber, pageSize);

        PagedResult<AutorVM> autoresPagedVM = new()
        {
            PageNumber = pagedResult.PageNumber,
            PageSize = pagedResult.PageSize,
            TotalItems = pagedResult.TotalItems,
            TotalPages = (int)Math.Ceiling(pagedResult.TotalItems / (double)pageSize),
            Items = pagedResult.Items.Select(autor => new AutorVM
            {
                Id = autor.Id,
                Nombre = autor.Nombre,
                Apellido = autor.Apellido,
                FechaNacimiento = autor.FechaNacimiento
            }).ToList()
        };

        return PartialView("_AutoresTabla", autoresPagedVM);
    }
}
