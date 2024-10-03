using BibliotecaNET8.Utils;

namespace BibliotecaNET8.Config;

public static class RouteConfig
{
    public static void RegisterRoutes(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapControllerRoute(
            name: "home",
            pattern: "/",
            defaults: new { controller = "Home", action = "Index" });

        endpoints.MapControllerRoute(
            name: "setLanguage",
            pattern: "/SetLanguage/{culture}",
            defaults: new { controller = "Home", action = "SetLanguage" });

        AutoresRoutes(endpoints);
        CategoriasRoutes(endpoints);
        ClientesRoutes(endpoints);
        LibrosRoutes(endpoints);
        PrestamosRoutes(endpoints);
    }

    #region AutoresRoutes
    public static void AutoresRoutes(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapControllerRoute(
            name: "autorIndex",
            pattern: "Autor/",
            defaults: new { controller = "Autor", action = "Index" });

        endpoints.MapControllerRoute(
            name: "autorCreate",
            pattern: "Autor/Create",
            defaults: new { controller = "Autor", action = "Create" });

        endpoints.MapControllerRoute(
            name: "autorEdit",
            pattern: "Autor/Edit/{id:int}",
            defaults: new { controller = "Autor", action = "Edit" });

        endpoints.MapControllerRoute(
            name: "autorVer",
            pattern: "Autor/Ver/{id:int}",
            defaults: new { controller = "Autor", action = "Ver" });

        endpoints.MapControllerRoute(
            name: "autorDelete",
            pattern: "Autor/Delete/{id:int}",
            defaults: new { controller = "Autor", action = "Delete" });

        endpoints.MapControllerRoute(
            name: "autorDeleteMultiple",
            pattern: "Autor/DeleteMultiple",
            defaults: new { controller = "Autor", action = "DeleteMultiple" });

        endpoints.MapControllerRoute(
            name: "autorSearch",
            pattern: "/Autor/Search/{term?}/{pageNumber}/{pageSize}",
            defaults: new { controller = "Autor", action = "Search",
                pageNumber = PaginationSettings.PageNumber, pageSize = PaginationSettings.PageSize });
    }
    #endregion

    #region CategoriasRoutes
    public static void CategoriasRoutes(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapControllerRoute(
            name: "categoriaIndex",
            pattern: "Categoria/",
            defaults: new { controller = "Categoria", action = "Index" });

        endpoints.MapControllerRoute(
            name: "categoriaCreate",
            pattern: "Categoria/Create",
            defaults: new { controller = "Categoria", action = "Create" });

        endpoints.MapControllerRoute(
            name: "categoriaEdit",
            pattern: "Categoria/Edit/{id:int}",
            defaults: new { controller = "Categoria", action = "Edit" });

        endpoints.MapControllerRoute(
            name: "categoriaVer",
            pattern: "Categoria/Ver/{id:int}",
            defaults: new { controller = "Categoria", action = "Ver" });

        endpoints.MapControllerRoute(
            name: "categoriaDelete",
            pattern: "Categoria/Delete/{id:int}",
            defaults: new { controller = "Categoria", action = "Delete" });

        endpoints.MapControllerRoute(
            name: "categoriaDeleteMultiple",
            pattern: "Categoria/DeleteMultiple",
            defaults: new { controller = "Categoria", action = "DeleteMultiple" });

        endpoints.MapControllerRoute(
            name: "categoriaSearch",
            pattern: "/Categoria/Search/{term?}/{pageNumber}/{pageSize}",
            defaults: new { controller = "Categoria", action = "Search",
                pageNumber = PaginationSettings.PageNumber, pageSize = PaginationSettings.PageSize
            });
    }
    #endregion

    #region ClientesRoutes
    public static void ClientesRoutes(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapControllerRoute(
            name: "clienteIndex",
            pattern: "Cliente/",
            defaults: new { controller = "Cliente", action = "Index" });

        endpoints.MapControllerRoute(
            name: "clienteCreate",
            pattern: "Cliente/Create",
            defaults: new { controller = "Cliente", action = "Create" });

        endpoints.MapControllerRoute(
            name: "clienteEdit",
            pattern: "Cliente/Edit/{id:int}",
            defaults: new { controller = "Cliente", action = "Edit" });

        endpoints.MapControllerRoute(
            name: "clienteVer",
            pattern: "Cliente/Ver/{id:int}",
            defaults: new { controller = "Cliente", action = "Ver" });

        endpoints.MapControllerRoute(
            name: "clienteDelete",
            pattern: "Cliente/Delete/{id:int}",
            defaults: new { controller = "Cliente", action = "Delete" });

        endpoints.MapControllerRoute(
            name: "clienteDeleteMultiple",
            pattern: "Cliente/DeleteMultiple",
            defaults: new { controller = "Cliente", action = "DeleteMultiple" });

        endpoints.MapControllerRoute(
            name: "clienteSearch",
            pattern: "/Cliente/Search/{term?}/{pageNumber}/{pageSize}",
            defaults: new { controller = "Cliente", action = "Search",
                pageNumber = PaginationSettings.PageNumber, pageSize = PaginationSettings.PageSize
            });
    }
    #endregion

    #region LibrosRoutes
    public static void LibrosRoutes(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapControllerRoute(
            name: "libroIndex",
            pattern: "Libro/",
            defaults: new { controller = "Libro", action = "Index" });

        endpoints.MapControllerRoute(
            name: "libroCreate",
            pattern: "Libro/Create",
            defaults: new { controller = "Libro", action = "Create" });

        endpoints.MapControllerRoute(
            name: "libroEdit",
            pattern: "Libro/Edit/{id:int}",
            defaults: new { controller = "Libro", action = "Edit" });

        endpoints.MapControllerRoute(
            name: "libroVer",
            pattern: "Libro/Ver/{id:int}",
            defaults: new { controller = "Libro", action = "Ver" });

        endpoints.MapControllerRoute(
            name: "libroDelete",
            pattern: "Libro/Delete/{id:int}",
            defaults: new { controller = "Libro", action = "Delete" });

        endpoints.MapControllerRoute(
            name: "libroDeleteMultiple",
            pattern: "Libro/DeleteMultiple",
            defaults: new { controller = "Libro", action = "DeleteMultiple" });

        endpoints.MapControllerRoute(
            name: "libroSearch",
            pattern: "/Libro/Search/{term?}/{pageNumber}/{pageSize}",
            defaults: new { controller = "Libro", action = "Search",
                pageNumber = PaginationSettings.PageNumber, pageSize = PaginationSettings.PageSize
            });
    }
    #endregion

    #region PrestamosRoutes
    public static void PrestamosRoutes(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapControllerRoute(
            name: "prestamoIndex",
            pattern: "Prestamo/",
            defaults: new { controller = "Prestamo", action = "Index" });

        endpoints.MapControllerRoute(
            name: "prestamoCreate",
            pattern: "Prestamo/Create",
            defaults: new { controller = "Prestamo", action = "Create" });

        endpoints.MapControllerRoute(
            name: "prestamoEdit",
            pattern: "Prestamo/Edit/{id:int}",
            defaults: new { controller = "Prestamo", action = "Edit" });

        endpoints.MapControllerRoute(
            name: "prestamoVer",
            pattern: "Prestamo/Ver/{id:int}",
            defaults: new { controller = "Prestamo", action = "Ver" });

        endpoints.MapControllerRoute(
            name: "prestamoDelete",
            pattern: "Prestamo/Delete/{id:int}",
            defaults: new { controller = "Prestamo", action = "Delete" });

        endpoints.MapControllerRoute(
            name: "prestamoDeleteMultiple",
            pattern: "Prestamo/DeleteMultiple",
            defaults: new { controller = "Prestamo", action = "DeleteMultiple" });

        endpoints.MapControllerRoute(
            name: "prestamoSearch",
            pattern: "/Prestamo/Search/{term?}/{pageNumber}/{pageSize}",
            defaults: new { controller = "Prestamo", action = "Search",
                pageNumber = PaginationSettings.PageNumber, pageSize = PaginationSettings.PageSize
            });
    }
    #endregion
}
