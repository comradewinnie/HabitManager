namespace HabitManager.Models
{
    public record UpdateHabitDto(
        Guid HabitId,
        string Title,
        string? Description,
        List<DayOfWeek> Periodicity
    );
}
