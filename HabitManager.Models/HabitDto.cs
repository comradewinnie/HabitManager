using System;
using System.Collections.Generic;
using System.Text;
using HabitManager.Domain;

namespace HabitManager.Models
{
    public record HabitDto(
        Guid Id,
        string UserId,  // tikai kamēr nav autentifikācijas
        string Title,
        string? Description,  // MI pateica, ka DTO, kas paredzēti lasīšanai vai attēlošanai, nevajag rakstīt null
        Status Status,
        List<DayOfWeek> Periodicity,
        List<CompletionRecordDto> History,
        DateTime CreatedAt,
        DateTime UpdatedAt
    );
}
