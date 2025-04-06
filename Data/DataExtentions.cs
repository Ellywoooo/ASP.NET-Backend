using Microsoft.EntityFrameworkCore;

namespace List.Data;

public static class DataExtentions
{
    public static async Task MigrateDbAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<GameStoreContexts>();
        await dbContext.Database.MigrateAsync();
    }
}
