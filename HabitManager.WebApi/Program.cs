// adaptēts pēc "UzdevumuParvaldnieks" piemēra

using HabitManager.Domain;
using HabitManager.TestData;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// GET all habits by user
app.MapGet("/api/users/{userId}/habits", (string userId) =>
{
    return DataGenerator.GetUserTestHabits(userId);
});

// GET habit by id
app.MapGet("/api/habits/{id}", (Guid id) =>
    DataGenerator.GetTestHabit(id)
        is Habit hb
        ? Results.Ok(hb)
        : Results.NotFound());

// POST new habit
app.MapPost("/api/habits", (Habit hb) =>
{
    Guid id = hb.Id;
    if (DataGenerator.AddTestHabit(hb))
    {
        return Results.Created($"/api/habits/{id}", hb);
    }
    else
    {
        return Results.BadRequest();
    }
});

// PUT existing habit
app.MapPut("/api/habits", (Habit hb) =>
{
    if (DataGenerator.UpdateTestHabit(hb))
        return Results.Ok();
    else return Results.NotFound();
});

// DELETE habit
app.MapDelete("/api/habits", (Guid id) =>
{
    if (DataGenerator.DeleteTestHabit(id))
        return Results.Ok();
    else return Results.NotFound();
});

app.Run();