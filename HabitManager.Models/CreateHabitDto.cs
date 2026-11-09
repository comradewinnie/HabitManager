using System.Text.Json.Serialization;

namespace HabitManager.Models
{
    public record CreateHabitDto(
        [property: JsonPropertyName("userId")] string UserId,  // tikai kamēr nav autentifikācijas
        [property: JsonPropertyName("title")] string Title,
        [property: JsonPropertyName("periodicity")] List<DayOfWeek> Periodicity,
        [property: JsonPropertyName("description")] string? Description = null
    );
}
