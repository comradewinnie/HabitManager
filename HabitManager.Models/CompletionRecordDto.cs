using System.Text.Json.Serialization;

namespace HabitManager.Models
{
    public record CompletionRecordDto(
        [property: JsonPropertyName("id")] Guid Id,
        [property: JsonPropertyName("habitId")] Guid HabitId,
        [property: JsonPropertyName("completedAt")] DateTime CompletedAt
    );
}
