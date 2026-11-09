// adaptēts pēc "UzdevumuParvaldnieks" piemēra

using HabitManager.Models;
using HabitManager.Shared.Mappings;
using HabitManager.TestData;
using System.Diagnostics;

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
    try
    {
        var domainHabits = DataGenerator.GetUserTestHabits(userId);
        if (domainHabits == null)
            return Results.NotFound("This user does not exist.");  // MI uzrakstīja tekstu kļūdai

        var dtoHabits = domainHabits.Select(hb => hb.ToDto()).ToList();  // MI saīsināja manu garu pierakstu ar foreach

        return Results.Ok(dtoHabits);
    }
    catch (Exception ex)
    {
        Debug.WriteLine($"GetUserTestHabits error: {ex.Message}");
        return Results.InternalServerError("Failed to load habits on server. Try later.");
    }
});

// GET habit by id
app.MapGet("/api/habits/{id}", (Guid id) =>
{
    try
    {
        var hb = DataGenerator.GetTestHabit(id);
        if (hb == null)
            return Results.NotFound("This habit does not exist.");

        return Results.Ok(hb.ToDto());
    }
    catch (Exception ex)
    {
        Debug.WriteLine($"GetTestHabit error: {ex.Message}");
        return Results.InternalServerError("Failed to load habit on server. Try later.");
    }
});

// GET habit completion history
app.MapGet("/api/habits/{id}/completion-history", (Guid id) =>
{
    try
    {
        var domainHistory = DataGenerator.GetTestHabitCompletionHistory(id);
        if (domainHistory == null)
            return Results.NotFound("This habit does not exist.");

        var dtoHistory = domainHistory.Select(record => record.ToDto()).ToList();

        return Results.Ok(dtoHistory);
    }
    catch (Exception ex)
    {
        Debug.WriteLine($"GetTestHabitCompletionHistory error: {ex.Message}");
        return Results.InternalServerError("Failed to load completion history on server. Try later.");
    }
});

// POST new habit
app.MapPost("/api/habits", (CreateHabitDto dto) =>
{
    try
    {
        var hb = dto.ToDomain();
        DataGenerator.AddTestHabit(hb);
        
        return Results.Created($"/api/habits/{hb.Id}", hb.ToDto());
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(ex.Message);
    }
    catch (Exception ex)
    {
        Debug.WriteLine($"AddTestHabit error: {ex.Message}");
        return Results.InternalServerError("Failed to create habit on server. Try later.");
    }
});

// POST new completion record to the habit
app.MapPost("/api/habits/{id}/complete", (MarkHabitAsCompleteDto dto) =>
{
    try
    {
        var hb = DataGenerator.GetTestHabit(dto.HabitId);
        if (hb == null)
            return Results.NotFound("This habit does not exist.");

        hb.MarkAsComplete(dto.CompletedAt);
        DataGenerator.UpdateTestHabit(hb);

        return Results.Ok();
    }
    catch (InvalidOperationException ex)
    {
        return Results.BadRequest(ex.Message);
    }
    catch (Exception ex)
    {
        Debug.WriteLine($"UpdateTestHabit error: {ex.Message}");
        return Results.InternalServerError("Failed to update habit on server. Try later.");
    }
});

// PUT existing habit
app.MapPut("/api/habits/{id}", (Guid id, UpdateHabitDto dto) =>
{
    if (id != dto.HabitId)  // MI pievienoja šo
        return Results.BadRequest("ID mismatch.");

    var hb = DataGenerator.GetTestHabit(dto.HabitId);
    if (hb == null)
        return Results.NotFound("This habit does not exist.");

    try
    {
        hb.UpdateDetails(dto.Title, dto.Description, dto.Periodicity);
        DataGenerator.UpdateTestHabit(hb);

        return Results.Ok();
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(ex.Message);
    }
    catch (Exception ex)
    {
        Debug.WriteLine($"UpdateTestHabit error: {ex.Message}");
        return Results.InternalServerError("Failed to update habit on server. Try later.");
    }
});

// PUT existing habit (toggle status)
app.MapPut("/api/habits/{id}/toggle-status", (Guid id) =>
{
    try
    {
        var hb = DataGenerator.GetTestHabit(id);
        if (hb == null)
            return Results.NotFound("This habit does not exist.");

        hb.ToggleHabitStatus();
        DataGenerator.UpdateTestHabit(hb);

        return Results.Ok();
    }
    catch (Exception ex)
    {
        Debug.WriteLine($"UpdateTestHabit error: {ex.Message}");
        return Results.InternalServerError("Failed to update habit on server. Try later.");
    }
});

// DELETE habit
app.MapDelete("/api/habits/{id}", (Guid id) =>
{
    try
    {
        if (DataGenerator.DeleteTestHabit(id))
            return Results.Ok();
        else return Results.NotFound("This habit does not exist.");
    }
    catch (Exception ex)
    {
        Debug.WriteLine($"DeleteTestHabit error: {ex.Message}");
        return Results.InternalServerError("Failed to delete habit on server. Try later.");
    }
});

app.Run();