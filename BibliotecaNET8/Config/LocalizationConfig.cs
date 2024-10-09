using Microsoft.AspNetCore.Localization;

namespace BibliotecaNET8.Web.Config;

/// <summary>
///     Configuración de la localización (i18n).
/// </summary>
public static class LocalizationConfig
{
    /// <summary>
    ///     Configura la localización para cargar las rutas donde se ubican las traducciones (.resx), 
    ///     con el soporte para multi-idioma
    /// </summary>
    /// <param name="services">La colección de servicios provenientes del contenedor de dependencias (Program.cs).</param>
    public static void ConfigureLocalization(this IServiceCollection services)
    {
        services.AddLocalization(options => options.ResourcesPath = "Resources");

        string[] supportedCultures = ["es", "en"];
        services.Configure<RequestLocalizationOptions>(options =>
        {
            options.DefaultRequestCulture = new RequestCulture("es");
            options.SetDefaultCulture("es")
                .AddSupportedCultures(supportedCultures)
                .AddSupportedUICultures(supportedCultures);

            options.RequestCultureProviders = new List<IRequestCultureProvider>
            {
                new QueryStringRequestCultureProvider(),
                new CookieRequestCultureProvider(),
                new AcceptLanguageHeaderRequestCultureProvider()
            };
        });
    }
}
