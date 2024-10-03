using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using BibliotecaNET8.ViewModels;
using BibliotecaNET8.ViewModels.Cliente;
using AutoMapper;
using BibliotecaNET8.Services;
using BibliotecaNET8.Utils;
using FluentValidation;
using FluentValidation.Results;
using FluentValidation.AspNetCore;
using BibliotecaNET8.Resources;
using BibliotecaNET8.Models;

namespace BibliotecaNET8.Controllers;

public class ClienteController : Controller
{
    private readonly IClienteService _clienteService;
    private readonly IStringLocalizer<Translations> _localizer;
    private readonly IMapper _mapper;
    private readonly IValidator<ClienteVM> _clienteValidator;

    public ClienteController(IClienteService clienteService, IStringLocalizer<Translations> localizer, IMapper mapper,
        IValidator<ClienteVM> clienteValidator)
    {
        _clienteService = clienteService;
        _localizer = localizer;
        _mapper = mapper;
        _clienteValidator = clienteValidator;
    }

    public async Task<IActionResult> Index(string? term = "", int pageNumber = PaginationSettings.PageNumber,
        int pageSize = PaginationSettings.PageSize)
    {
        term = term?.ToLower().Trim();
        ViewData["CurrentSearchTerm"] = term;

        PagedResult<ClienteVM> clientesPagedVM = new();
        IQueryable<Cliente> clientes = await _clienteService.GetAllClientes();
        if (!string.IsNullOrEmpty(term))
        {
            clientes = _clienteService.SearchCliente(clientes, cliente =>
                cliente.Nombre.ToLower().Contains(term) ||
                cliente.Apellido.ToLower().Contains(term) ||
                cliente.Email.ToLower().Contains(term) ||
                cliente.Telefono.ToLower().Contains(term));
        }

        if (!clientes.Any())
        {
            ViewBag.ListaClientes = _localizer["ListaClientesEmptyMessage"];
        }
        else
        {
            PagedResult<Cliente> pagedResult = await _clienteService.GetRecordsPagedResult(clientes, pageNumber, pageSize);

            clientesPagedVM = new PagedResult<ClienteVM>
            {
                PageNumber = pagedResult.PageNumber,
                PageSize = pagedResult.PageSize,
                TotalItems = pagedResult.TotalItems,
                TotalPages = (int)Math.Ceiling(pagedResult.TotalItems / (double)pageSize),
                Items = pagedResult.Items.Select(cliente => new ClienteVM
                {
                    Id = cliente.Id,
                    Nombre = cliente.Nombre,
                    Apellido = cliente.Apellido,
                    Email = cliente.Email,
                    Telefono = cliente.Telefono
                }).ToList()
            };
        }

        return View(clientesPagedVM);
    }

    [HttpGet]
    public IActionResult Create() => View();

    [HttpPost]
    public async Task<IActionResult> Create(ClienteVM clienteVM)
    {
        ValidationResult result = await _clienteValidator.ValidateAsync(clienteVM);
        if (result.IsValid)
        {
            try
            {
                Cliente? cliente = _mapper.Map<ClienteVM, Cliente>(clienteVM);
                await _clienteService.AddCliente(cliente);
                TempData["ClientesMensaje"] = _localizer["ClienteCreadoMessageSuccess"].Value;

                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                return View("404");
            }
        }

        result.AddToModelState(ModelState);

        return View(clienteVM);
    }

    [HttpGet]
    public async Task<IActionResult> Ver(int? id)
    {
        try
        {
            Cliente? cliente = await _clienteService.GetClienteById(id);
            ClienteVM clienteVM = _mapper.Map<Cliente, ClienteVM>(cliente);

            return View(clienteVM);
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
            Cliente? cliente = await _clienteService.GetClienteById(id);
            ClienteVM clienteVM = _mapper.Map<Cliente, ClienteVM>(cliente);

            return View(clienteVM);
        }
        catch (Exception)
        {
            return View("404");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Edit(ClienteVM clienteVM)
    {
        ValidationResult result = await _clienteValidator.ValidateAsync(clienteVM);
        if (result.IsValid)
        {
            Cliente? cliente = _mapper.Map<ClienteVM, Cliente>(clienteVM);
            await _clienteService.UpdateCliente(cliente);
            TempData["ClientesMensaje"] = _localizer["ClienteModificadoMessageSuccess"].Value;

            return RedirectToAction(nameof(Index));
        }

        result.AddToModelState(ModelState);

        return View(clienteVM);
    }

    [HttpPost]
    public async Task<JsonResult> Delete(int? id)
    {
        bool isDeleted;
        try
        {
            isDeleted = await _clienteService.DeleteCliente(id);
            if (isDeleted)
            {
                TempData["ClientesMensaje"] = _localizer["ClienteEliminadoMessageSuccess"].Value;
            }
            else
            {
                TempData["ClientesMensaje"] = _localizer["ClienteEliminadoMessageFail"].Value;
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al eliminar el cliente: {ex.Message}");
        }

        return Json(new
        {
            success = isDeleted,
            mensaje = TempData["ClientesMensaje"]
        });
    }

    [HttpPost]
    public async Task<JsonResult> DeleteMultiple([FromBody] int[] idsCliente)
    {
        bool isDeleted;
        try
        {
            isDeleted = await _clienteService.DeleteMultipleClientes(idsCliente);
            if (isDeleted)
            {
                TempData["ClientesMensaje"] = _localizer["ClienteEliminadoMultipleMessageSuccess"].Value;
            }
            else
            {
                TempData["ClientesMensaje"] = _localizer["ClienteEliminadoMultipleMessageFail"].Value;
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al eliminar múltiples clientes: {ex.Message}");
        }

        return Json(new
        {
            success = isDeleted,
            mensaje = TempData["ClientesMensaje"]
        });
    }

    public async Task<IActionResult> Search(string? term = "", int pageNumber = PaginationSettings.PageNumber,
        int pageSize = PaginationSettings.PageSize)
    {
        term = (term == PaginationSettings.ShowAllRecordsText) ? string.Empty : term?.ToLower().Trim();
        ViewData["CurrentSearchTerm"] = term;
        IQueryable<Cliente> filtroClientes;
        try
        {
            filtroClientes = _clienteService.SearchCliente(cliente =>
                cliente.Nombre.ToLower().Contains(term) ||
                cliente.Apellido.ToLower().Contains(term) ||
                cliente.Email.ToLower().Contains(term) ||
                cliente.Telefono.ToLower().Contains(term));
        }
        catch (Exception)
        {
            throw new Exception("Error al buscar categorías");
        }

        PagedResult<Cliente> pagedResult = await _clienteService.GetRecordsPagedResult(filtroClientes, pageNumber, pageSize);

        PagedResult<ClienteVM> clientesPagedVM = new()
        {
            PageNumber = pagedResult.PageNumber,
            PageSize = pagedResult.PageSize,
            TotalItems = pagedResult.TotalItems,
            TotalPages = (int)Math.Ceiling(pagedResult.TotalItems / (double)pageSize),
            Items = pagedResult.Items.Select(cliente => new ClienteVM
            {
                Id = cliente.Id,
                Nombre = cliente.Nombre,
                Apellido = cliente.Apellido,
                Email = cliente.Email,
                Telefono = cliente.Telefono
            }).ToList()
        };

        return PartialView("_ClientesTabla", clientesPagedVM);
    }
}
