using System.Text.Json.Serialization;

namespace HabitManager.Models
{
    public record UpdateHabitDto(
        [property: JsonPropertyName("habitId")] Guid HabitId,
        [property: JsonPropertyName("title")] string Title,
        [property: JsonPropertyName("description")] string? Description,
        [property: JsonPropertyName("periodicity")] List<DayOfWeek> Periodicity
    );
}
