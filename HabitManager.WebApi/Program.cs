// adaptēts pēc "UzdevumuParvaldnieks" piemēra

using HabitManager.Models;
using HabitManager.Shared.Mappings;
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
    var domainHabits = DataGenerator.GetUserTestHabits(userId);
    if (domainHabits == null)
        return Results.NotFound(new { message = "User not found" });  // MI uzrakstīja tekstu kļūdai

    var dtoHabits = domainHabits.Select(hb => hb.ToDto()).ToList();  // MI saīsināja manu garu pierakstu ar foreach

    return Results.Ok(dtoHabits);
});

// GET habit by id
app.MapGet("/api/habits/{id}", (Guid id) =>
{
    var hb = DataGenerator.GetTestHabit(id);
    if (hb == null)
        return Results.NotFound();

    return Results.Ok(hb.ToDto());
});

// GET habit completion history
app.MapGet("/api/habits/{id}/completion-history", (Guid id) =>
{
    var domainHistory = DataGenerator.GetTestHabitCompletionHistory(id);
    if (domainHistory == null)
        return Results.NotFound();

    var dtoHistory = domainHistory.Select(record => record.ToDto()).ToList();

    return Results.Ok(dtoHistory);
});

// POST new habit
app.MapPost("/api/habits", (CreateHabitDto dto) =>
{
    var hb = dto.ToDomain();
    if (DataGenerator.AddTestHabit(hb))
    {
        return Results.Created($"/api/habits/{hb.Id}", hb.ToDto());
    }
    else
    {
        return Results.BadRequest();
    }
});

// POST new completion record to the habit
app.MapPost("/api/habits/{id}/complete", (MarkHabitAsCompleteDto dto) =>
{
    var hb = DataGenerator.GetTestHabit(dto.HabitId);
    if (hb == null)
        return Results.NotFound();

    hb.MarkAsComplete(dto.CompletedAt);
    DataGenerator.UpdateTestHabit(hb);

    return Results.Ok();
});

// PUT existing habit
app.MapPut("/api/habits", (UpdateHabitDto dto) =>
{
    var hb = DataGenerator.GetTestHabit(dto.HabitId);
    if (hb == null)
        return Results.NotFound();

    hb.UpdateDetails(dto.Title, dto.Description, dto.Periodicity);
    DataGenerator.UpdateTestHabit(hb);

    return Results.Ok();
});

// PUT existing habit (toggle status)
app.MapPut("/api/habits/{id}/toggle-status", (Guid id) =>
{
    var hb = DataGenerator.GetTestHabit(id);
    if (hb == null)
        return Results.NotFound();

    hb.ToggleHabitStatus();
    DataGenerator.UpdateTestHabit(hb);

    return Results.Ok();
});

// DELETE habit
app.MapDelete("/api/habits", (Guid id) =>
{
    if (DataGenerator.DeleteTestHabit(id))
        return Results.Ok();
    else return Results.NotFound();
});

app.Run();