using AutoMapper;
using BibliotecaNET8.Application.DTOs.Autor;
using BibliotecaNET8.Application.Services.Interfaces;
using BibliotecaNET8.Domain;
using BibliotecaNET8.Domain.Entities;
using BibliotecaNET8.Domain.Exceptions;
using BibliotecaNET8.Domain.UnitOfWork.Interfaces;
using BibliotecaNET8.Infrastructure.Utils;
using BibliotecaNET8.Web.ViewModels.Autor;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace BibliotecaNET8.Web.Controllers;

/// <summary>
///     Funcionalidades de la vista "Autor"
/// </summary>
public class AutorController : Controller
{
    private readonly IAutorService _autorService;
    private readonly IStringLocalizer<Translations> _localizer;
    private readonly IMapper _mapper;
    private readonly IValidator<AutorDTO> _autorValidator;
    private readonly IUnitOfWork _unitOfWork;

    public AutorController(IAutorService autorService,IStringLocalizer<Translations> localizer, IMapper mapper,
        IValidator<AutorDTO> autorValidator, IUnitOfWork unitOfWork)
    {
        _autorService = autorService;
        _localizer = localizer;
        _mapper = mapper;
        _autorValidator = autorValidator;
        _unitOfWork = unitOfWork;
    }

    public async Task<IActionResult> Index(string? term = "", int pageNumber = PaginationSettings.PageNumber,
        int pageSize = PaginationSettings.PageSize)
    {
        IQueryable<Autor> autores = await _autorService.GetAllAutores();
        PagedResult<AutorVM> autoresPagedVM = new();

        term = term?.ToLower().Trim();
        ViewData["CurrentSearchTerm"] = term;
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
            autoresPagedVM = _mapper.Map<PagedResult<AutorVM>>(source: pagedResult);
        }

        return View(autoresPagedVM);
    }

    [HttpGet]
    public IActionResult Create() => View();

    [HttpPost]
    public async Task<IActionResult> Create(AutorVM autorVM)
    {
        AutorDTO autorDTO = _mapper.Map<AutorDTO>(source: autorVM);
        var result = await _autorValidator.ValidateAsync(autorDTO);
        if (result.IsValid)
        {
            try
            {
                Autor autor = _mapper.Map<Autor>(source: autorDTO);
                await _autorService.AddAutor(autor);
                await _unitOfWork.Save();
                TempData["AutoresMensaje"] = _localizer["AutorCreadoMessageSuccess"].Value;

                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                return NotFound();
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
            if (autor == null)
            {
                return NotFound();
            }

            AutorVM autorVM = _mapper.Map<AutorVM>(source: autor);
            return View(autorVM);
        }
        catch (Exception)
        {
            return NotFound();
        }
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int? id)
    {
        try
        {
            Autor autor = await _autorService.GetAutorById(id);
            if (autor == null)
            {
                return NotFound();
            }

            AutorVM autorVM = _mapper.Map<AutorVM>(source: autor);
            return View(autorVM);
        }
        catch (Exception)
        {
            return NotFound();
        }
    }

    [HttpPost]
    public async Task<IActionResult> Edit(AutorVM autorVM)
    {
        AutorDTO autorDTO = _mapper.Map<AutorDTO>(source: autorVM);
        var result = await _autorValidator.ValidateAsync(autorDTO);
        if (result.IsValid)
        {
            try
            {
                Autor autor = _mapper.Map<Autor>(source: autorDTO);
                await _autorService.UpdateAutor(autor);
                await _unitOfWork.Save();
                TempData["AutoresMensaje"] = _localizer["AutorModificadoMessageSuccess"].Value;

                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        result.AddToModelState(ModelState);

        return View(autorVM);
    }

    [HttpPost]
    public async Task<JsonResult> Delete(int? id)
    {
        bool isDeleted;
        string message;
        try
        {
            isDeleted = await _autorService.DeleteAutor(id);
            if (isDeleted)
            {
                message = _localizer["AutorEliminadoMessageSuccess"].Value;
            }
            else
            {
                message = _localizer["AutorEliminadoMessageFail"].Value;
            }

            await _unitOfWork.Save();
            TempData["AutoresMensaje"] = message;
        }
        catch (Exception ex)
        {
            throw new CRUDException($"Error al eliminar el autor: {ex.Message}");
        }

        return Json(new
        {
            success = isDeleted,
            mensaje = message
        });
    }

    [HttpPost]
    public async Task<JsonResult> DeleteMultiple([FromBody] int[] idsAutor)
    {
        bool isDeleted;
        string message;
        try
        {
            isDeleted = await _autorService.DeleteMultipleAutores(idsAutor);
            if (isDeleted)
            {
                message = _localizer["AutorEliminadoMultipleMessageSuccess"].Value;
            }
            else
            {
                message = _localizer["AutorEliminadoMultipleMessageFail"].Value;
            }

            await _unitOfWork.Save();
            TempData["AutoresMensaje"] = message;
        }
        catch (Exception ex)
        {
            throw new CRUDException($"Error al eliminar múltiples autores: {ex.Message}");
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
        IQueryable<Autor> filtroAutores;
        try
        {
            filtroAutores = _autorService.SearchAutor(autor =>
                autor.Nombre.ToLower().Contains(term) ||
                autor.Apellido.ToLower().Contains(term));
        }
        catch (Exception)
        {
            throw new SearchException("Error al buscar autores");
        }

        PagedResult<Autor> pagedResult = await _autorService.GetRecordsPagedResult(filtroAutores, pageNumber, pageSize);
        PagedResult<AutorVM> autoresPagedVM = _mapper.Map<PagedResult<AutorVM>>(source: pagedResult);

        return PartialView("_AutoresTabla", autoresPagedVM);
    }
}
