using System;
using List.Data;
using List.Mapping;
using Microsoft.EntityFrameworkCore;

namespace List.EndPoints;

public static class GenresEndPoints
{
    public static RouteGroupBuilder MapGenresEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("genres");

        group.MapGet("/", async (GameStoreContexts dbContext) =>
            await dbContext.Genres
                            .Select(genre => genre.ToDto())
                            .AsNoTracking()
                            .ToListAsync());
        return group;
    }
}
