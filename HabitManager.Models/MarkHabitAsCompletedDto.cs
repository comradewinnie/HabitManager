using System.Text.Json.Serialization;

namespace HabitManager.Models
{
    public record MarkHabitAsCompleteDto(
        [property: JsonPropertyName("habitId")] Guid HabitId,
        [property: JsonPropertyName("completedAt")] DateTime CompletedAt
    );
}
