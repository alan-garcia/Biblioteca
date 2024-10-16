using BibliotecaNET8.Web.ViewModels;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaNET8.Web.Controllers;

/// <summary>
///     Funcionalidades de la vista "Home"
/// </summary>
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    /// <summary>
    ///     Aplica el idioma en el menú principal de la vista, y guarda su selección en la cookie.
    /// </summary>
    /// <param name="culture">Prefijo del idioma a aplicar.</param>
    /// <param name="returnUrl">URL después de aplicar el idioma en la vista.</param>
    /// <returns>Redirección URL a aplicar después de seleccionar el idioma.</returns>
    public IActionResult SetLanguage(string culture, string returnUrl)
    {
        Response.Cookies.Append(
            CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
            new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
        );

        return LocalRedirect(returnUrl ?? "/");
    }

    /// <summary>
    ///     Maneja los errores generales (500, excepciones no controladas).
    /// </summary>
    /// <returns>La vista con el error.</returns>
    public IActionResult Error()
    {
        var error = HttpContext.Features.Get<IExceptionHandlerFeature>();
        return View(
            new ErrorVM { Message = "Ocurrió un problema", ErrorDetails = error.Error.Message }
        );
    }

    /// <summary>
    ///     Maneja los códigos de estado.
    /// </summary>
    /// <param name="code">Número correspondiente al código de estado.</param>
    /// <returns>Nombre de la vista correspondiente al código de estado.</returns>
    public IActionResult StatusCode(int code)
    {
        switch (code)
        {
            case 401:
                return View("401");
            case 403:
                return View("403");
            case 404:
                return View("404",
                    new ErrorVM { Message = "La página solicitada no existe", StatusCode = code }
                );
            default:
                return View(
                    "GeneralError",
                    new ErrorVM { Message = "Ocurrió un problema", StatusCode = code }
                );
        }
    }
}
