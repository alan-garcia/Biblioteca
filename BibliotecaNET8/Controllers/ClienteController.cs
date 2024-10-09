using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using BibliotecaNET8.Application.Services.Interfaces;
using BibliotecaNET8.Infrastructure.Utils;
using BibliotecaNET8.Domain.Entities;
using BibliotecaNET8.Domain;
using BibliotecaNET8.Web.ViewModels.Cliente;
using BibliotecaNET8.Application.DTOs.Cliente;
using BibliotecaNET8.Domain.UnitOfWork.Interfaces;

namespace BibliotecaNET8.Web.Controllers;

/// <summary>
///     Funcionalidades de la vista "Cliente"
/// </summary>
public class ClienteController : Controller
{
    private readonly IClienteService _clienteService;
    private readonly IStringLocalizer<Translations> _localizer;
    private readonly IMapper _mapper;
    private readonly IValidator<ClienteDTO> _clienteValidator;
    private readonly IUnitOfWork _unitOfWork;

    public ClienteController(IClienteService clienteService, IStringLocalizer<Translations> localizer, IMapper mapper,
        IValidator<ClienteDTO> clienteValidator, IUnitOfWork unitOfWork)
    {
        _clienteService = clienteService;
        _localizer = localizer;
        _mapper = mapper;
        _clienteValidator = clienteValidator;
        _unitOfWork = unitOfWork;
    }

    public async Task<IActionResult> Index(string? term = "", int pageNumber = PaginationSettings.PageNumber,
        int pageSize = PaginationSettings.PageSize)
    {
        IQueryable<Cliente> clientes = await _clienteService.GetAllClientes();
        PagedResult<ClienteVM> clientesPagedVM = new();
        
        term = term?.ToLower().Trim();
        ViewData["CurrentSearchTerm"] = term;
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
            clientesPagedVM = _mapper.Map<PagedResult<ClienteVM>>(source: pagedResult);
        }

        return View(clientesPagedVM);
    }

    [HttpGet]
    public IActionResult Create() => View();

    [HttpPost]
    public async Task<IActionResult> Create(ClienteVM clienteVM)
    {
        ClienteDTO clienteDTO = _mapper.Map<ClienteDTO>(source: clienteVM);
        var result = await _clienteValidator.ValidateAsync(clienteDTO);
        if (result.IsValid)
        {
            try
            {
                Cliente cliente = _mapper.Map<Cliente>(source: clienteDTO);
                await _clienteService.AddCliente(cliente);
                await _unitOfWork.Save();
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
            ClienteVM clienteVM = _mapper.Map<ClienteVM>(source: cliente);

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
            ClienteVM clienteVM = _mapper.Map<ClienteVM>(source: cliente);

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
        ClienteDTO clienteDTO = _mapper.Map<ClienteDTO>(source: clienteVM);
        var result = await _clienteValidator.ValidateAsync(clienteDTO);
        if (result.IsValid)
        {
            Cliente? cliente = _mapper.Map<Cliente>(source: clienteDTO);
            await _clienteService.UpdateCliente(cliente);
            await _unitOfWork.Save();
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
        string message;
        try
        {
            isDeleted = await _clienteService.DeleteCliente(id);
            if (isDeleted)
            {
                message = _localizer["ClienteEliminadoMessageSuccess"].Value;
            }
            else
            {
                message = _localizer["ClienteEliminadoMessageFail"].Value;
            }

            await _unitOfWork.Save();
            TempData["ClientesMensaje"] = message;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al eliminar el cliente: {ex.Message}");
        }

        return Json(new
        {
            success = isDeleted,
            mensaje = message
        });
    }

    [HttpPost]
    public async Task<JsonResult> DeleteMultiple([FromBody] int[] idsCliente)
    {
        bool isDeleted;
        string message;
        try
        {
            isDeleted = await _clienteService.DeleteMultipleClientes(idsCliente);
            if (isDeleted)
            {
                message = _localizer["ClienteEliminadoMultipleMessageSuccess"].Value;
            }
            else
            {
                message = _localizer["ClienteEliminadoMultipleMessageFail"].Value;
            }

            await _unitOfWork.Save();
            TempData["ClientesMensaje"] = message;
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
        PagedResult<ClienteVM> clientesPagedVM = _mapper.Map<PagedResult<ClienteVM>>(source: pagedResult);

        return PartialView("_ClientesTabla", clientesPagedVM);
    }
}
