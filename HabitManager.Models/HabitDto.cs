using HabitManager.Domain;
using System.Text.Json.Serialization;

namespace HabitManager.Models
{
    public record HabitDto(
        [property: JsonPropertyName("id")] Guid Id,
        [property: JsonPropertyName("userId")] string UserId,  // tikai kamēr nav autentifikācijas
        [property: JsonPropertyName("title")] string Title,
        [property: JsonPropertyName("description")] string? Description,  // MI pateica, ka DTO, kas paredzēti lasīšanai vai attēlošanai, nevajag rakstīt null
        [property: JsonPropertyName("status")] Status Status,
        [property: JsonPropertyName("periodicity")] List<DayOfWeek> Periodicity,
        [property: JsonPropertyName("history")] List<CompletionRecordDto> History,
        [property: JsonPropertyName("createdAt")] DateTime CreatedAt,
        [property: JsonPropertyName("updatedAt")] DateTime UpdatedAt
    );
}
