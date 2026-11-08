namespace HabitManager.Models
{
    public record CreateHabitDto(
        string UserId,  // tikai kamēr nav autentifikācijas
        string Title,
        string? Description = null,
        List<DayOfWeek> Periodicity
    );
}
