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
}
