using List.Data;
using List.Dtos;
using List.EndPoints;

var builder = WebApplication.CreateBuilder(args);

var connString = builder.Configuration.GetConnectionString("GameStore");

builder.Services.AddSqlite<GameStoreContexts>(connString);


var app = builder.Build();

app.MapGamesEndPoints(); 

app.MapGenresEndpoints();

await app.MigrateDbAsync();

app.Run();
