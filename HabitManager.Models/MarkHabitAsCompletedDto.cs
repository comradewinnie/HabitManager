namespace HabitManager.Models
{
    public record MarkHabitAsCompleteDto(
        Guid HabitId,
        DateTime CompletedAt
    );
}
