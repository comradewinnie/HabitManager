// adaptēts pēc "UzdevumuParvaldnieks" piemēra

using HabitManager.Data;
using HabitManager.Models;
using HabitManager.Shared.Mappings;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IHabitRepository, EfHabitRepository>();

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())  // izveidoja MI
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();

        context.Database.Migrate();  // automātiski veic migrācijas

        DbSeeder.Seed(services);  // uzpilda datubāzi ar testa datiem, ja tā ir tukša
    }
    catch (Exception ex)
    {
        Debug.WriteLine($"An error occured while processing database: {ex.Message}");
    }
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// GET all habits by user
app.MapGet("/api/users/{userId}/habits", async (string userId, IHabitRepository repository) =>
{
    try
    {
        var domainHabits = await repository.GetUserHabitsAsync(userId);
        if (domainHabits == null)
            return Results.NotFound("This user does not exist.");  // MI uzrakstīja tekstu kļūdai

        var dtoHabits = domainHabits.Select(hb => hb.ToDto()).ToList();  // MI saīsināja manu garu pierakstu ar foreach

        return Results.Ok(dtoHabits);
    }
    catch (Exception ex)
    {
        Debug.WriteLine($"GetUserHabitsAsync error: {ex.Message}");
        return Results.InternalServerError("Failed to load habits on server. Try later.");
    }
});

// GET habit by id
app.MapGet("/api/habits/{id}", async (Guid id, IHabitRepository repository) =>
{
    try
    {
        var hb = await repository.GetHabitAsync(id);
        if (hb == null)
            return Results.NotFound("This habit does not exist.");

        return Results.Ok(hb.ToDto());
    }
    catch (Exception ex)
    {
        Debug.WriteLine($"GetHabitAsync error: {ex.Message}");
        return Results.InternalServerError("Failed to load habit on server. Try later.");
    }
});

// GET habit completion history
app.MapGet("/api/habits/{id}/completion-history", async (Guid id, IHabitRepository repository) =>
{
    try
    {
        var domainHistory = await repository.GetHabitCompletionHistoryAsync(id);
        if (domainHistory == null)
            return Results.NotFound("This habit does not exist.");

        var dtoHistory = domainHistory.Select(record => record.ToDto()).ToList();

        return Results.Ok(dtoHistory);
    }
    catch (Exception ex)
    {
        Debug.WriteLine($"GetHabitCompletionHistoryAsync error: {ex.Message}");
        return Results.InternalServerError("Failed to load completion history on server. Try later.");
    }
});

// POST new habit
app.MapPost("/api/habits", async (CreateHabitDto dto, IHabitRepository repository) =>
{
    try
    {
        var hb = dto.ToDomain();
        await repository.AddHabitAsync(hb);

        return Results.Created($"/api/habits/{hb.Id}", hb.ToDto());
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(ex.Message);
    }
    catch (Exception ex)
    {
        Debug.WriteLine($"AddHabitAsync error: {ex.Message}");
        return Results.InternalServerError("Failed to create habit on server. Try later.");
    }
});

// POST new completion record to the habit
app.MapPost("/api/habits/{id}/complete", async (MarkHabitAsCompleteDto dto, IHabitRepository repository) =>
{
    try
    {
        if (await repository.AddCompletionRecordAsync(dto.HabitId, dto.CompletedAt))
            return Results.Ok();
        else
            return Results.NotFound("This habit does not exist.");
    }
    catch (InvalidOperationException ex)
    {
        return Results.BadRequest(ex.Message);
    }
    catch (Exception ex)
    {
        Debug.WriteLine($"AddCompletionRecordAsync error: {ex.Message}");
        return Results.InternalServerError("Failed to update habit on server. Try later.");
    }
});

// PUT existing habit
app.MapPut("/api/habits/{id}", async (Guid id, UpdateHabitDto dto, IHabitRepository repository) =>
{
    if (id != dto.HabitId)  // MI pievienoja šo
        return Results.BadRequest("ID mismatch.");

    try
    {
        if (await repository.UpdateHabitDetailsAsync(dto.HabitId, dto.Title, dto.Description, dto.Periodicity))
            return Results.Ok();
        else
            return Results.NotFound("This habit does not exist.");
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(ex.Message);
    }
    catch (Exception ex)
    {
        Debug.WriteLine($"UpdateHabitDetailsAsync error: {ex.Message}");
        return Results.InternalServerError("Failed to update habit on server. Try later.");
    }
});

// PUT existing habit (toggle status)
app.MapPut("/api/habits/{id}/toggle-status", async (Guid id, IHabitRepository repository) =>
{
    try
    {
        if (await repository.UpdateHabitStatusAsync(id))
            return Results.Ok();
        else
            return Results.NotFound("This habit does not exist.");
    }
    catch (Exception ex)
    {
        Debug.WriteLine($"UpdateHabitStatusAsync error: {ex.Message}");
        return Results.InternalServerError("Failed to update habit on server. Try later.");
    }
});

// DELETE habit
app.MapDelete("/api/habits/{id}", async (Guid id, IHabitRepository repository) =>
{
    try
    {
        if (await repository.DeleteHabitAsync(id))
            return Results.Ok();
        else return Results.NotFound("This habit does not exist.");
    }
    catch (Exception ex)
    {
        Debug.WriteLine($"DeleteHabitAsync error: {ex.Message}");
        return Results.InternalServerError("Failed to delete habit on server. Try later.");
    }
});

app.Run();