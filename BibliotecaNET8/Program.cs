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

// Configura el middleware de localización
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

/* Middleware que intercepta los códigos de estado, y redirige a una acción en el controlador "Home"
 * llamada "StatusCode", pasando el código de estado como un parámetro. */
app.UseStatusCodePagesWithReExecute("/StatusCode/{0}");

app.UseExceptionHandler("/Home/Error");
app.UseAuthorization();

app.UseMiddleware<ErrorDatabaseMiddleware>();

// Registra las rutas de la aplicación
app.RegisterRoutes();

app.Run();
