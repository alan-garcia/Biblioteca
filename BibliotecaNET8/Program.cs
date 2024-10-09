using BibliotecaNET8.Application.MappingProfiles;
using BibliotecaNET8.Infrastructure;
using BibliotecaNET8.Web.Config;
using BibliotecaNET8.Web.MappingProfiles;
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
builder.Services.AddAutoMapper(typeof(MappingProfiles));
builder.Services.AddAutoMapper(typeof(ViewModelProfile));
//builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

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
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.RegisterRoutes();

app.Run();
