using BibliotecaNET8.Application.MappingProfiles;
using BibliotecaNET8.Infrastructure;
using BibliotecaNET8.Web.Config;
using BibliotecaNET8.Web.MappingProfiles;
using BibliotecaNET8.Web.Middlewares;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews()
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization();

builder.Services.ConnectToDatabase(builder.Configuration);
builder.Services.ConfigureLocalization();
builder.Services.RegisterServicesAndRepositories();
builder.Services.RegisterValidators();

// Configura todos los Profiles de AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfiles));
builder.Services.AddAutoMapper(typeof(ViewModelProfile));

var app = builder.Build();

// Configura el middleware de localizaci�n
var localizationOptions = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value;
app.UseRequestLocalization(localizationOptions);

if (app.Environment.IsDevelopment())
{
    app.InitSeeds();
    app.ClearSeedsWhenStops();
}
else
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

/* Middleware que intercepta los c�digos de estado, y redirige a una acci�n en el controlador "Home"
 * llamada "StatusCode", pasando el c�digo de estado como un par�metro. */
app.UseStatusCodePagesWithReExecute("/StatusCode/{0}");

app.UseExceptionHandler("/Home/Error");
app.UseAuthorization();

app.UseMiddleware<ErrorDatabaseMiddleware>();

// Registra las rutas de la aplicaci�n
app.RegisterRoutes();

app.Run();
