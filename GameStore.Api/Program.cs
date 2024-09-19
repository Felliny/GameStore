using GameStore.Api.Dtos;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

const string GetGameEndpointName = "GetGame";


List<GameDto> games = [
    new (
        1,
        "Katana Collector",
        "Metroidvania",
        20.00M,
        new DateOnly(2028, 3, 20)
    ),
    new (
        2,
        "Katana Collector 2",
        "Metroidvania",
        25.00M,
        new DateOnly(2034, 3, 20)
    ),
    new (
        3,
        "Hollow Knight Silksong",
        "Metroidvania",
        50.00M,
        new DateOnly(2500, 7, 4)
    ),
];

// GET /games
app.MapGet("games", () => games);

// GET /games/1
app.MapGet("games/{id}", (int id) => 
{
    GameDto? game = games.Find(game => game.Id == id);

    return game is null ? Results.NotFound() : Results.Ok(game);
})
.WithName(GetGameEndpointName);

// POST /games
app.MapPost("games", (CreateGameDto newGame) => 
{
    GameDto game = new(
        games.Count + 1,
        newGame.Name,
        newGame.Genre,
        newGame.Price,
        newGame.ReleaseDate
    );

    games.Add(game);

    return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id}, game);
});

// PUT /games/1
app.MapPut("games/{id}", (int id, UpdateGameDto updateGame) =>
{
    var index = games.FindIndex(game => game.Id == id);

    if (index == -1){
        return Results.NotFound();
    }

    games[index] = new GameDto(
        id,
        updateGame.Name,
        updateGame.Genre,
        updateGame.Price,
        updateGame.ReleaseDate
    );

    return Results.NoContent();
});

//DELETE /games/1
app.MapDelete("games/{id}", (int id) =>
{
    games.RemoveAll(game => game.Id == id);

    return Results.NoContent();
});

app.Run();
