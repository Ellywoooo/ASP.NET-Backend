using System;
using List.Data;
using List.Dtos;
using List.Entities;
using List.Mapping;
using Microsoft.EntityFrameworkCore;

namespace List.EndPoints;

public static class GamesEndPoints
{
    
    const string GetGameEndPointName = "GetGame";

   
    public static RouteGroupBuilder MapGamesEndPoints(this WebApplication app)
    {
        var group = app.MapGroup("games")
                        .WithParameterValidation();

        //GET /games
        group.MapGet("/", async (GameStoreContexts dbContext) => 
        {
            
            return await dbContext.Games
                     .Include(game => game.Genre)
                     .Select(game => game.ToGameSummaryDto())
                     .AsNoTracking()
                     .ToListAsync();
        });


        //GET /games/1
        group.MapGet("/{id}", async (int id, GameStoreContexts dbContext) =>
        {
            Game? game = await dbContext.Games.FindAsync(id);
            
            return game is null ? 
                Results.NotFound() : Results.Ok(game.ToGameDetailsDto());
        })
        .WithName(GetGameEndPointName);

        //POST /games
        group.MapPost("/", async (CreateGameDto newGame, GameStoreContexts dbContext) =>
        {
        
        Game game = newGame.ToEntity();

            dbContext.Games.Add(game);
            await dbContext.SaveChangesAsync(); 


            return Results.CreatedAtRoute( 
                GetGameEndPointName,
                new {id = game.Id}, 
                game.ToGameDetailsDto());
        });

        //PUT /games 
        group.MapPut("/{id}", async (int id, UpdateGameDto updatedGame, GameStoreContexts dbContext) =>
        {
            var existingGame = await dbContext.Games.FindAsync(id);

            if (existingGame is null)
            {
                return Results.NotFound();
            }
            
            dbContext.Entry(existingGame)
                     .CurrentValues
                     .SetValues(updatedGame.ToEntity(id));

            await dbContext.SaveChangesAsync();

            return Results.NoContent();
            });
  
            //DELETE /games/1
            group.MapDelete("/{id}", async (int id, GameStoreContexts dbContext) =>
            {
                await dbContext.Games
                         .Where(game => game.Id == id)
                         .ExecuteDeleteAsync();

                return Results.NoContent();
            });

            return group;
    }
}
