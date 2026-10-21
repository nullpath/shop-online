namespace ShopOnline.Api.Endpoints;

internal static class AppEndpoints
{
    internal static void MapEndpoints(this WebApplication app)
    {
        app.MapCategoryEndpoints();
        app.MapSubcategoryEndpoints();
        app.MapSizeEndpoints();
        app.MapColorEndpoints();
        app.MapCollectionEndpoints();
        app.MapGenderEndpoints();
        app.MapProductEndpoints();
    }
}
