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
using BibliotecaNET8.ViewModels.Categoria;
using BibliotecaNET8.ViewModels.Autor;

namespace BibliotecaNET8.Controllers;

public class CategoriaController : Controller
{
    private readonly ICategoriaService _categoriaService;
    private readonly IStringLocalizer<Translations> _localizer;
    private readonly IMapper _mapper;
    private readonly IValidator<CategoriaVM> _categoriaValidator;

    public CategoriaController(ICategoriaService categoriaService, IStringLocalizer<Translations> localizer, IMapper mapper,
        IValidator<CategoriaVM> categoriaValidator)
    {
        _categoriaService = categoriaService;
        _localizer = localizer;
        _mapper = mapper;
        _categoriaValidator = categoriaValidator;
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
            categoriasPagedVM = _mapper.Map<PagedResult<Categoria>, PagedResult<CategoriaVM>>(pagedResult);
        }

        return View(categoriasPagedVM);
    }

    [HttpGet]
    public IActionResult Create() => View();

    [HttpPost]
    public async Task<IActionResult> Create(CategoriaVM categoriaVM)
    {
        ValidationResult result = await _categoriaValidator.ValidateAsync(categoriaVM);
        if (result.IsValid)
        {
            try
            {
                Categoria? categoria = _mapper.Map<CategoriaVM, Categoria>(categoriaVM);
                await _categoriaService.AddCategoria(categoria);
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
            CategoriaVM categoriaVM = _mapper.Map<Categoria, CategoriaVM>(categoria);

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
            CategoriaVM categoriaVM = _mapper.Map<Categoria, CategoriaVM>(categoria);

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
        ValidationResult result = await _categoriaValidator.ValidateAsync(categoriaVM);
        if (result.IsValid)
        {
            Categoria categoria = _mapper.Map<CategoriaVM, Categoria>(categoriaVM);
            await _categoriaService.UpdateCategoria(categoria);
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
        try
        {
            isDeleted = await _categoriaService.DeleteCategoria(id);
            if (isDeleted)
            {
                TempData["CategoriaMensajes"] = _localizer["CategoriaEliminadaMessageSuccess"].Value;
            }
            else
            {
                TempData["CategoriaMensajes"] = _localizer["CategoriaEliminadaMessageFail"].Value;
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al eliminar la categoría: {ex.Message}");
        }

        return Json(new
        {
            success = isDeleted,
            mensaje = TempData["CategoriaMensajes"]
        });
    }

    [HttpPost]
    public async Task<JsonResult> DeleteMultiple([FromBody] int[] idsCategoria)
    {
        bool isDeleted;
        try
        {
            isDeleted = await _categoriaService.DeleteMultipleCategorias(idsCategoria);
            if (isDeleted)
            {
                TempData["CategoriaMensajes"] = _localizer["CategoriaEliminadaMultipleMessageSuccess"].Value;
            }
            else
            {
                TempData["CategoriaMensajes"] = _localizer["CategoriaEliminadaMultipleMessageFail"].Value;
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al eliminar múltiples categoría: {ex.Message}");
        }

        return Json(new
        {
            success = isDeleted,
            mensaje = TempData["CategoriaMensajes"]
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
        PagedResult<CategoriaVM> categoriasPagedVM = _mapper.Map<PagedResult<Categoria>, PagedResult<CategoriaVM>>(pagedResult);

        return PartialView("_CategoriasTabla", categoriasPagedVM);
    }
}
