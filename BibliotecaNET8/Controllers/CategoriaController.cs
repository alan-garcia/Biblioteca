using AutoMapper;
using BibliotecaNET8.Application.DTOs.Categoria;
using BibliotecaNET8.Application.Services.Interfaces;
using BibliotecaNET8.Domain;
using BibliotecaNET8.Domain.Entities;
using BibliotecaNET8.Domain.UnitOfWork.Interfaces;
using BibliotecaNET8.Infrastructure.Utils;
using BibliotecaNET8.Web.ViewModels.Categoria;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace BibliotecaNET8.Web.Controllers;

/// <summary>
///     Funcionalidades de la vista "Categoria"
/// </summary>
public class CategoriaController : Controller
{
    private readonly ICategoriaService _categoriaService;
    private readonly IStringLocalizer<Translations> _localizer;
    private readonly IMapper _mapper;
    private readonly IValidator<CategoriaDTO> _categoriaValidator;
    private readonly IUnitOfWork _unitOfWork;

    public CategoriaController(ICategoriaService categoriaService, IStringLocalizer<Translations> localizer, IMapper mapper,
        IValidator<CategoriaDTO> categoriaValidator, IUnitOfWork unitOfWork)
    {
        _categoriaService = categoriaService;
        _localizer = localizer;
        _mapper = mapper;
        _categoriaValidator = categoriaValidator;
        _unitOfWork = unitOfWork;
    }

    public async Task<IActionResult> Index(string? term = "", int pageNumber = PaginationSettings.PageNumber,
        int pageSize = PaginationSettings.PageSize)
    {
        IQueryable<Categoria> categorias = await _categoriaService.GetAllCategorias();
        PagedResult<CategoriaVM> categoriasPagedVM = new();
        
        term = term?.ToLower().Trim();
        ViewData["CurrentSearchTerm"] = term;
        if (!string.IsNullOrEmpty(term))
        {
            categorias = _categoriaService.SearchCategoria(categorias, categoria =>
                categoria.Nombre.ToLower().Contains(term));
        }

        if (!categorias.Any())
        {
            ViewBag.ListaCategorias = _localizer["ListaCategoriasEmptyMessage"];
        }
        else
        {
            PagedResult<Categoria> pagedResult = await _categoriaService.GetRecordsPagedResult(categorias, pageNumber, pageSize);
            categoriasPagedVM = _mapper.Map<PagedResult<CategoriaVM>>(source: pagedResult);
        }

        return View(categoriasPagedVM);
    }

    [HttpGet]
    public IActionResult Create() => View();

    [HttpPost]
    public async Task<IActionResult> Create(CategoriaVM categoriaVM)
    {
        CategoriaDTO categoriaDTO = _mapper.Map<CategoriaDTO>(source: categoriaVM);
        var result = await _categoriaValidator.ValidateAsync(categoriaDTO);
        if (result.IsValid)
        {
            try
            {
                Categoria? categoria = _mapper.Map<Categoria>(source: categoriaDTO);
                await _categoriaService.AddCategoria(categoria);
                await _unitOfWork.Save();
                TempData["CategoriasMensaje"] = _localizer["CategoriaCreadaMessageSuccess"].Value;

                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                return View("404");
            }
        }

        result.AddToModelState(ModelState);

        return View(categoriaVM);
    }

    [HttpGet]
    public async Task<IActionResult> Ver(int? id)
    {
        try
        {
            Categoria categoria = await _categoriaService.GetCategoriaById(id);
            CategoriaVM categoriaVM = _mapper.Map<CategoriaVM>(source: categoria);

            return View(categoriaVM);
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
            Categoria categoria = await _categoriaService.GetCategoriaById(id);
            CategoriaVM categoriaVM = _mapper.Map<CategoriaVM>(source: categoria);

            return View(categoriaVM);
        }
        catch (Exception)
        {
            return View("404");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Edit(CategoriaVM categoriaVM)
    {
        CategoriaDTO categoriaDTO = _mapper.Map<CategoriaDTO>(source: categoriaVM);
        var result = await _categoriaValidator.ValidateAsync(categoriaDTO);
        if (result.IsValid)
        {
            Categoria categoria = _mapper.Map<Categoria>(source: categoriaDTO);
            await _categoriaService.UpdateCategoria(categoria);
            await _unitOfWork.Save();
            TempData["CategoriasMensaje"] = _localizer["CategoriaModificadaMessageSuccess"].Value;

            return RedirectToAction(nameof(Index));
        }

        result.AddToModelState(ModelState);

        return View(categoriaVM);
    }

    [HttpPost]
    public async Task<JsonResult> Delete(int? id)
    {
        bool isDeleted;
        string message;
        try
        {
            isDeleted = await _categoriaService.DeleteCategoria(id);
            if (isDeleted)
            {
                message = _localizer["CategoriaEliminadaMessageSuccess"].Value;
            }
            else
            {
                message = _localizer["CategoriaEliminadaMessageFail"].Value;
            }

            await _unitOfWork.Save();
            TempData["CategoriaMensajes"] = message;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al eliminar la categoría: {ex.Message}");
        }

        return Json(new
        {
            success = isDeleted,
            mensaje = message
        });
    }

    [HttpPost]
    public async Task<JsonResult> DeleteMultiple([FromBody] int[] idsCategoria)
    {
        bool isDeleted;
        string message;
        try
        {
            isDeleted = await _categoriaService.DeleteMultipleCategorias(idsCategoria);
            if (isDeleted)
            {
                message = _localizer["CategoriaEliminadaMultipleMessageSuccess"].Value;
            }
            else
            {
                message = _localizer["CategoriaEliminadaMultipleMessageFail"].Value;
            }

            await _unitOfWork.Save();
            TempData["CategoriaMensajes"] = message;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al eliminar múltiples categoría: {ex.Message}");
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
        IQueryable<Categoria> filtroCategorias;
        try
        {
            filtroCategorias = _categoriaService.SearchCategoria(categoria =>
                categoria.Nombre.ToLower().Contains(term));
        }
        catch (Exception)
        {
            throw new Exception("Error al buscar categorías");
        }

        PagedResult<Categoria> pagedResult = await _categoriaService.GetRecordsPagedResult(filtroCategorias, pageNumber, pageSize);
        PagedResult<CategoriaVM> categoriasPagedVM = _mapper.Map<PagedResult<CategoriaVM>>(source: pagedResult);

        return PartialView("_CategoriasTabla", categoriasPagedVM);
    }
}
